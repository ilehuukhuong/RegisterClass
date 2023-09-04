using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using System.Web;
using API.Data.Template;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper, IUnitOfWork uow, IMailService mailService, IPhotoService photoService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _uow = uow;
            _mailService = mailService;
            _photoService = photoService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await EmailExists(registerDto.Email)) return BadRequest("Email already in use");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = await _uow.UserRepository.UsernameGenerator();
            user.Email = registerDto.Email.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Student");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            if (registerDto.File != null)
            {
                var resultPhoto = await _photoService.AddPhotoAsync(registerDto.File);

                if (resultPhoto.Error != null) return BadRequest(resultPhoto.Error.Message);

                user.PhotoUrl = resultPhoto.SecureUrl.AbsoluteUri;
                user.PhotoId = resultPhoto.PublicId;

                await _uow.Complete();
            }
           
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            loginDto.Username = loginDto.Username.ToLower();
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Username);

            if (user == null) return BadRequest("Invalid Username or Email");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result) return BadRequest("Invalid Password");

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email.ToLower());

            if (user == null) return BadRequest("User not found.");

            //if (user.EmailConfirmed == false) return BadRequest("This email is not confirmed yet. Please confirm your email.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = HttpUtility.UrlEncode(resetToken);
            var encodedEmail = HttpUtility.UrlEncode(user.Email);

            var resetUrl = $"https://domain/reset-password/email{encodedEmail}&token{encodedToken}";

            var emailContent = ResetPasswordTemplate.ResetPassword(resetUrl, user.FirstName);

            await _mailService.SendEmailAsync(user.Email, "Reset Password", emailContent);

            return Ok("Password reset link sent to your email.");
        }

        [HttpPut("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email.ToLower());

            if (user == null) return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Password reset successfully. Please login again.");
            }
            else
            {
                return BadRequest("Problem when resetting password.");
            }
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}

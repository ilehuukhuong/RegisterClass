using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;
        public AdminController(UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork uow, IPhotoService photoService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _uow = uow;
            _photoService = photoService;
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-class")]
        public async Task<ActionResult> DeleteClass(int classId, string username)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            var Class = await _uow.ClassRepository.GetClassById(classId);
            if (Class == null) return NotFound();

            var studentClass = await _uow.UserRepository.GetStudentClassForEntityAsync(user, Class);

            if (studentClass == null) return NotFound();

            _uow.UserRepository.RemoveStudentClass(studentClass);

            if (await _uow.Complete()) return Ok("Delete successfully");

            return BadRequest("Failed to delete");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("add-class")]
        public async Task<ActionResult> AddClass(int classId, string username)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            var Class = await _uow.ClassRepository.GetClassById(classId);
            if (Class == null) return NotFound();

            if (await _uow.UserRepository.GetStudentClassForEntityAsync(user, Class) != null) return BadRequest("You already in this class");

            var studentClass = new StudentClass
            {
                User = user,
                UserId = user.Id,
                Class = Class,
                ClassId = Class.Id
            };

            _uow.UserRepository.AddStudentClass(studentClass);

            if (await _uow.Complete()) return Ok("Add to class succesfully.");

            return BadRequest("Failed to add to class");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("update-student/{studentId}")]
        public async Task<ActionResult<StudentDto>> UpdateStudent(int studentId, [FromForm]RegisterDto registerDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(studentId);
            if (user == null) return NotFound();

            registerDto.Email = registerDto.Email.ToLower();

            _mapper.Map(registerDto, user);
            user.NormalizedEmail = registerDto.Email.ToUpper();

            if (registerDto.File != null)
            {
                var resultPhoto = await _photoService.AddPhotoAsync(registerDto.File);

                if (resultPhoto.Error != null) return BadRequest(resultPhoto.Error.Message);
                if (user.PhotoId != null)
                {
                    var resultDeletePhoto = await _photoService.DeletePhotoAsync(user.PhotoId);
                    if (resultDeletePhoto.Error != null) return BadRequest(resultDeletePhoto.Error.Message);
                }

                user.PhotoUrl = resultPhoto.SecureUrl.AbsoluteUri;
                user.PhotoId = resultPhoto.PublicId;

                await _userManager.UpdateAsync(user);
            }

            return Ok(_mapper.Map<StudentDto>(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-student")]
        public async Task<ActionResult<StudentDto>> CreateStudent([FromForm]RegisterDto registerDto)
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

            return Ok(_mapper.Map<StudentDto>(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create-teacher/{username}")]
        public async Task<ActionResult<TeacherDto>> CreateTeacher(string username,[FromForm]CreateTeacherDto createTeacherDto)
        {
            if (await UserExists(username)) return BadRequest("Username already in use");

            var user = _mapper.Map<AppUser>(createTeacherDto);

            user.UserName = username.ToLower();
            user.Email = createTeacherDto.Email.ToLower();

            var result = await _userManager.CreateAsync(user, createTeacherDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Teacher");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            if (createTeacherDto.File != null)
            {
                var resultPhoto = await _photoService.AddPhotoAsync(createTeacherDto.File);

                if (resultPhoto.Error != null) return BadRequest(resultPhoto.Error.Message);

                user.PhotoUrl = resultPhoto.SecureUrl.AbsoluteUri;
                user.PhotoId = resultPhoto.PublicId;

                await _uow.Complete();
            }

            return Ok(_mapper.Map<TeacherDto>(user));
        }

        [Authorize(Policy = "RequireTeacherRole")]
        [HttpGet("get-students")]
        public async Task<ActionResult<PagedList<StudentDto>>> GetStudents([FromQuery] UserParams userParams)
        {
            var users = await _uow.UserRepository.GetStudents(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("get-teachers")]
        public async Task<ActionResult<PagedList<TeacherDto>>> GetTeachers([FromQuery] UserParams userParams)
        {
            var users = await _uow.UserRepository.GetTeachers(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User Id is required.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with Id {userId} not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok($"User with Id {userId} has been deleted.");
            }

            return BadRequest($"Failed to delete user with Id {userId}.");
        }

        private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}

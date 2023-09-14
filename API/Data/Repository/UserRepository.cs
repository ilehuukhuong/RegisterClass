using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Interfaces;
using API.DTOs;
using AutoMapper.QueryableExtensions;
using API.Helpers;

namespace API.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddStudentClass(StudentClass studentClass)
        {
            _context.StudentClasses.Add(studentClass);
        }

        public void RemoveStudentClass(StudentClass studentClass)
        {
            _context.StudentClasses.Remove(studentClass);
        }

        public Task<StudentClass> GetStudentClassForEntityAsync(AppUser user, Class Class)
        {
            return _context.StudentClasses.FirstOrDefaultAsync(x => x.User == user && x.Class == Class);
        }

        public async Task<PagedList<StudentDto>> GetStudents(UserParams userParams)
        {
            var role = await _context.Roles.Where(x => x.Name == "Student").FirstOrDefaultAsync();
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserRoles.Any(r => r.RoleId == role.Id));
            if (userParams.Search != null)
            {
                query = query.Where(u => u.UserName.Contains(userParams.Search) || u.Email.Contains(userParams.Search) || u.PhoneNumber.Contains(userParams.Search) || u.FirstName.Contains(userParams.Search) || u.LastName.Contains(userParams.Search));
            }

            return await PagedList<StudentDto>.CreateAsync(query.AsNoTracking().ProjectTo<StudentDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<TeacherDto>> GetTeachers(UserParams userParams)
        {
            var role = await _context.Roles.Where(x => x.Name == "Teacher").FirstOrDefaultAsync();
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserRoles.Any(r => r.RoleId == role.Id));
            if (userParams.Search != null)
            {
                query = query.Where(u => u.UserName.Contains(userParams.Search) || u.Email.Contains(userParams.Search) || u.PhoneNumber.Contains(userParams.Search) || u.FirstName.Contains(userParams.Search) || u.LastName.Contains(userParams.Search));
            }

            return await PagedList<TeacherDto>.CreateAsync(query.AsNoTracking().ProjectTo<TeacherDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> UsernameGenerator()
        {
            var studentRoleId = await _context.Roles
                .Where(r => r.Name == "Student")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var latestUser = await _context.UserRoles
                .Where(ur => ur.RoleId == studentRoleId)
                .Join(_context.Users,
                      ur => ur.UserId,
                      u => u.Id,
                      (ur, u) => u)
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync();

            int latestCounter = 0;
            if (latestUser != null)
            {
                int year = int.Parse(latestUser.UserName.Substring(0, 4));
                int month = int.Parse(latestUser.UserName.Substring(4, 2));

                if (year == DateTime.Now.Year && month == DateTime.Now.Month)
                {
                    latestCounter = int.Parse(latestUser.UserName.Substring(7)) + 1;
                }
                else latestCounter = 1;
            }
            else latestCounter = 1;

            string username = $"{DateTime.Now.Year:D4}{DateTime.Now.Month:D2}-{latestCounter:D5}";

            return username;
        }
    }
}

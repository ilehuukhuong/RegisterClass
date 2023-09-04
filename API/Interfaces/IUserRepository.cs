using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<StudentDto>> GetStudents(UserParams userParams);
        Task<PagedList<TeacherDto>> GetTeachers(UserParams userParams);
        Task<string> UsernameGenerator();
    }
}

using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void AddStudentClass(StudentClass studentClass);
        void RemoveStudentClass(StudentClass studentClass);
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<StudentClass> GetStudentClassForEntityAsync(AppUser user, Class Class);
        Task<PagedList<StudentDto>> GetStudents(UserParams userParams);
        Task<PagedList<TeacherDto>> GetTeachers(UserParams userParams);
        Task<string> UsernameGenerator();
    }
}

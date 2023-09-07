using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ICourseRepository
    {
        void AddCourse(Course course);
        bool DeleteCourse(Course course);
        void UpdateCourse(Course course);
        Task<Course> GetCourseById(int id);
        Task<Course> GetCourseByCode(string code);
        Task<PagedList<CourseDto>> GetCourses(SearchParams searchParams);
    }
}

using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IGradeCategoryRepository
    {
        void AddGradeCategory(GradeCategory gradeCategory);
        bool DeleteGradeCategory(int id);
        void UpdateGradeCategory(GradeCategory gradeCategory);
        void CreateCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory);
        Task UpdateCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory);
        void DeleteCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory);
        Task<PagedList<CourseGradeCategoryDto>> GetCourseGradeCategories(SearchParams searchParams);
        Task<GradeCategory> GetGradeCategoryById(int id);
        Task<GradeCategory> GetGradeCategoryByName(string name);
        Task<IEnumerable<GradeCategory>> GetGradeCategories();
    }
}

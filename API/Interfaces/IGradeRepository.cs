using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IGradeRepository
    {
        void AddGrade(Grade grade);
        bool DeleteGrade(int id);
        void UpdateGrade(Grade Grade);
        Task<Grade> GetGradeById(int id);
        Task<PagedList<StudentGradeDto>> GetGrades(SearchParams searchParams);
    }
}

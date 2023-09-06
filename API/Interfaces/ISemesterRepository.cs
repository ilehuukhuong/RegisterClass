using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ISemesterRepository
    {
        void AddSemester(SemesterDto semesterDto);
        void UpdateSemester(SemesterDto semesterDto);
        bool DeleteSemester(Semester semester);
        SemesterDto CopySemester(Semester semester);
        Task<Semester> GetSemesterById(int id);
        Task<Semester> GetSemesterByCode(string code);
        Task<Semester> GetSemesterByName(string name);
        Task<PagedList<SemesterDto>> GetSemesters(SearchParams searchParams);
    }
}

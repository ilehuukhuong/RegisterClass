using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IDepartmentFacultyRepository
    {
        void AddDepartmentFaculty(DepartmentFaculty departmentFaculty);
        bool DeleteDepartmentFaculty(int id);
        void UpdateDepartmentFaculty(DepartmentFaculty departmentFaculty);
        Task<DepartmentFaculty> GetDepartmentFacultyById(int id);
        Task<DepartmentFaculty> GetDepartmentFacultyByName(string name);
        Task<PagedList<DepartmentFacultyDto>> GetDepartmentFaculties(SearchParams searchParams);
    }
}

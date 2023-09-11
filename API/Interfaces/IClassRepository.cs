using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IClassRepository
    {
        void AddClass(Class Class);
        bool DeleteClass(Class Class);
        Task<Class> GetClassById(int id);
        Task<Class> GetClassByCode(string code);
        Task<PagedList<ClassDto>> GetClasses(SearchParams searchParams);
    }
}

using API.Entities;

namespace API.Interfaces
{
    public interface ISalaryRepository
    {
        void AddSalary(Salary salary);
        bool DeleteSalary(int id);
        void UpdateSalary(Salary salary);
        Task<Salary> GetSalaryById(int id);
    }
}

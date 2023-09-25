using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class SalaryRepository : ISalaryRepository
    {
        private readonly DataContext _context;
        public SalaryRepository(DataContext context)
        {
            _context = context;
        }

        public void AddSalary(Salary Salary)
        {
            _context.Salaries.Add(Salary);
        }

        public bool DeleteSalary(int id)
        {
            _context.Salaries.Remove(_context.Salaries.FirstOrDefault(x => x.Id == id));

            return true;
        }

        public async Task<Salary> GetSalaryById(int id)
        {
            return await _context.Salaries.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateSalary(Salary Salary)
        {
            _context.Salaries.Update(Salary);
        }
    }
}

using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class DepartmentFacultyRepository : IDepartmentFacultyRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DepartmentFacultyRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddDepartmentFaculty(DepartmentFaculty departmentFaculty)
        {
            _context.DepartmentFaculties.Add(departmentFaculty);
        }

        public bool DeleteDepartmentFaculty(int id)
        {
            if (_context.Courses.FirstOrDefault(x => x.DepartmentFacultyId == id) != null) return false;

            _context.DepartmentFaculties.Remove(_context.DepartmentFaculties.FirstOrDefault(x => x.Id == id));

            return true;
        }

        public async Task<PagedList<DepartmentFacultyDto>> GetDepartmentFaculties(SearchParams searchParams)
        {
            var query = _context.DepartmentFaculties.AsQueryable();

            if (searchParams.Search != null)
            {
                query = query.Where(u => u.Name.Contains(searchParams.Search));
            }

            return await PagedList<DepartmentFacultyDto>.CreateAsync(query.AsNoTracking().ProjectTo<DepartmentFacultyDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public async Task<DepartmentFaculty> GetDepartmentFacultyById(int id)
        {
            return await _context.DepartmentFaculties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<DepartmentFaculty> GetDepartmentFacultyByName(string name)
        {
            return await _context.DepartmentFaculties.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public void UpdateDepartmentFaculty(DepartmentFaculty departmentFaculty)
        {
            _context.DepartmentFaculties.Update(departmentFaculty);
        }
    }
}

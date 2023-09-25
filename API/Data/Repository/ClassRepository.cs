using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ClassRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddClass(Class Class)
        {
            _context.Classes.Add(Class);
        }

        public bool DeleteClass(Class Class)
        {
            // (_context.Classs.FirstOrDefault(x => x.SemesterId == semester.Id) != null) return false;

            _context.Classes.Remove(Class);

            return true;
        }

        public async Task<Class> GetClassById(int classid)
        {
            return await _context.Classes.FirstOrDefaultAsync(x => x.Id == classid);
        }

        public async Task<Class> GetClassByCode(string code)
        {
            return await _context.Classes.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<PagedList<ClassDto>> GetClasses(SearchParams searchParams)
        {
            var query = _context.Classes.AsQueryable();

            if (searchParams.Search != null) query = query.Where(u => u.Name.Contains(searchParams.Search));

            if (searchParams.Code != null) query = query.Where(u => u.Code.Contains(searchParams.Code));

            if (searchParams.SemesterId != null) query = query.Where(u => u.SemesterId == searchParams.SemesterId);

            if (searchParams.DepartmentFacultyId != null) query = query.Where(u => u.DepartmentFacultyId == searchParams.DepartmentFacultyId);

            return await PagedList<ClassDto>.CreateAsync(query.AsNoTracking().ProjectTo<ClassDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }
    }
}

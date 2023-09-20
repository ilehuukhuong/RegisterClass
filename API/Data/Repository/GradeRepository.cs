using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class GradeRepository : IGradeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public GradeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGrade(Grade grade)
        {
            _context.Grades.Add(grade);
        }

        public bool DeleteGrade(int id)
        {
            _context.Grades.Remove(_context.Grades.FirstOrDefault(x => x.Id == id));

            return true;
        }

        public async Task<Grade> GetGradeById(int id)
        {
            return await _context.Grades.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<StudentGradeDto>> GetGrades(SearchParams searchParams)
        {
            var query = _context.Grades.AsQueryable();

            query = query.Where(u => u.ClassId == searchParams.ClassId.Value);

            query = query.Where(u => u.CourseId == searchParams.CourseId.Value);

            if (searchParams.Search != null) query = query.Where(u => (u.AppUser.LastName.ToLower() + " " + u.AppUser.FirstName.ToLower()).Contains(searchParams.Search.ToLower()));

            query = query.OrderBy(u => u.AppUser.FirstName);

            return await PagedList<StudentGradeDto>.CreateAsync(query.AsNoTracking().ProjectTo<StudentGradeDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public void UpdateGrade(Grade grade)
        {
            _context.Grades.Update(grade);
        }
    }
}

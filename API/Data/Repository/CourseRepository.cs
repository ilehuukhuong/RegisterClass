using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CourseRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
        }

        public bool DeleteCourse(Course course)
        {
            // (_context.Courses.FirstOrDefault(x => x.SemesterId == semester.Id) != null) return false;

            _context.Courses.Remove(course);

            return true;
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Course> GetCourseByCode(string code)
        {
            return await _context.Courses.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<PagedList<CourseDto>> GetCourses(SearchParams searchParams)
        {
            var query = _context.Courses.AsQueryable();

            if (searchParams.Search != null) query = query.Where(u => u.Name.Contains(searchParams.Search) || u.Code.Contains(searchParams.Search));

            if (searchParams.SemesterId != null) query = query.Where(u => u.SemesterId == searchParams.SemesterId);

            if (searchParams.DepartmentFacultyId != null) query = query.Where(u => u.DepartmentFacultyId == searchParams.DepartmentFacultyId);

            return await PagedList<CourseDto>.CreateAsync(query.AsNoTracking().ProjectTo<CourseDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
        }
    }
}

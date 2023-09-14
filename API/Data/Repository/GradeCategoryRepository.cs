using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class GradeCategoryRepository : IGradeCategoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public GradeCategoryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddGradeCategory(GradeCategory gradeCategory)
        {
            _context.GradeCategories.Add(gradeCategory);
        }

        public void UpdateGradeCategory(GradeCategory gradeCategory)
        {
            _context.GradeCategories.Update(gradeCategory);
        }

        public bool DeleteGradeCategory(int id)
        {
            //if (_context.Users.FirstOrDefault(x => x.GradeCategoryId == id) != null) return false;

            _context.GradeCategories.Remove(_context.GradeCategories.FirstOrDefault(x => x.Id == id));

            return true;
        }

        public async Task<GradeCategory> GetGradeCategoryById(int id)
        {
            return await _context.GradeCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<GradeCategory> GetGradeCategoryByName(string name)
        {
            return await _context.GradeCategories.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<GradeCategory>> GetGradeCategories()
        {
            return await _context.GradeCategories.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<PagedList<CourseGradeCategoryDto>> GetCourseGradeCategories(SearchParams searchParams)
        {
            var query = _context.CourseGradeCategories.AsQueryable();

            if (searchParams.Search != null) query = query.Where(u => u.Course.Name.Contains(searchParams.Search));

            return await PagedList<CourseGradeCategoryDto>.CreateAsync(query.AsNoTracking().ProjectTo<CourseGradeCategoryDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public void CreateCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory)
        {
            _context.CourseGradeCategories.Add(_mapper.Map<CourseGradeCategory>(createCourseGradeCategory));
        }

        public async Task UpdateCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory)
        {
            var courseGradeCategory = await _context.CourseGradeCategories.FirstOrDefaultAsync(x => x.CourseId == createCourseGradeCategory.CourseId && x.GradeCategoryId == createCourseGradeCategory.GradeCategoryId);
            _mapper.Map(createCourseGradeCategory, courseGradeCategory);
        }

        public void DeleteCourseGradeCategory(CreateCourseGradeCategory createCourseGradeCategory)
        {
            _context.CourseGradeCategories.Remove(_context.CourseGradeCategories.FirstOrDefault(x => x.CourseId == createCourseGradeCategory.CourseId && x.GradeCategoryId == createCourseGradeCategory.GradeCategoryId));
        }
    }
}

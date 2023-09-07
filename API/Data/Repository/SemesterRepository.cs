using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SemesterRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddSemester(SemesterDto semester)
        {
            semester.Id = 0;
            _context.Semesters.Add(_mapper.Map<Semester>(semester));
        }

        public void UpdateSemester(SemesterDto semesterDto)
        {
            _mapper.Map(semesterDto, _context.Semesters.FirstOrDefault(x => x.Id == semesterDto.Id));
        }

        public bool DeleteSemester(Semester semester)
        {
            if (_context.Courses.FirstOrDefault(x => x.SemesterId == semester.Id) != null) return false;

            _context.Semesters.Remove(semester);

            return true;
        }

        public async Task<Semester> GetSemesterById(int id)
        {
            return await _context.Semesters.FirstOrDefaultAsync(x => x.Id == id);
        }

        public SemesterDto CopySemester (Semester semester)
        {
            var copy = new SemesterDto();
            copy.Code = semester.Code + "Copy";
            copy.Name = semester.Name + " Copy";
            copy.StartDay = semester.StartDay;
            copy.EndDay = semester.EndDay;
            return copy;
        }

        public async Task<Semester> GetSemesterByName(string name)
        {
            return await _context.Semesters.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<Semester> GetSemesterByCode(string code)
        {
            return await _context.Semesters.FirstOrDefaultAsync(x => x.Code.ToLower() == code.ToLower());
        }

        public async Task<PagedList<SemesterDto>> GetSemesters(SearchParams searchParams)
        {
            var query = _context.Semesters.AsQueryable();

            if (searchParams.Search != null)
            {
                query = query.Where(u => u.Code.Contains(searchParams.Search) || u.Name.Contains(searchParams.Search));
            }

            return await PagedList<SemesterDto>.CreateAsync(query.AsNoTracking().ProjectTo<SemesterDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }
    }
}

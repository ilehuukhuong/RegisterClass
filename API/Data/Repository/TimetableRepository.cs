using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class TimetableRepository : ITimetableRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public TimetableRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddTimetable(Timetable Timetable)
        {
            _context.Timetables.Add(Timetable);
        }

        public bool DeleteTimetable(Timetable Timetable)
        {

            _context.Timetables.Remove(Timetable);

            return true;
        }

        public async Task<Timetable> GetTimetableById(int id)
        {
            return await _context.Timetables.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<TimetableDto>> GetTimetables(SearchParams searchParams)
        {
            var query = _context.Timetables.AsQueryable();

            if (searchParams.Code != null) query = query.Where(u => u.AppUser.UserName.Contains(searchParams.Code));

            if (searchParams.Search != null) query = query.Where(u => u.AppUser.FirstName.Contains(searchParams.Search) || u.AppUser.LastName.Contains(searchParams.Search));

            if (searchParams.CourseName != null) query = query.Where(u => u.Course.Name.Contains(searchParams.CourseName));

            return await PagedList<TimetableDto>.CreateAsync(query.AsNoTracking().ProjectTo<TimetableDto>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public void UpdateTimetable(Timetable Timetable)
        {
            _context.Timetables.Update(Timetable);
        }
    }
}

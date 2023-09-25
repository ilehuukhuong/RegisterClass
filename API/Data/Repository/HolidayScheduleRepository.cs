using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class HolidayScheduleRepository : IHolidayScheduleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public HolidayScheduleRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddHolidaySchedule(HolidaySchedule holidaySchedule)
        {
            _context.HolidaySchedules.Add(holidaySchedule);
        }

        public bool DeleteHolidaySchedule(int id)
        {
            _context.HolidaySchedules.Remove(_context.HolidaySchedules.FirstOrDefault(x => x.Id == id));

            return true;
        }

        public async Task<HolidaySchedule> GetHolidayScheduleById(int id)
        {
            return await _context.HolidaySchedules.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<HolidaySchedule>> GetHolidaySchedules(SearchParams searchParams)
        {
            var query = _context.HolidaySchedules.AsQueryable();

            if (searchParams.Search != null) query = query.Where(u => u.Name.Contains(searchParams.Search) || u.Description.Contains(searchParams.Search));
            query = query.OrderByDescending(x => x.Id);

            return await PagedList<HolidaySchedule>.CreateAsync(query.AsNoTracking().ProjectTo<HolidaySchedule>(_mapper.ConfigurationProvider), searchParams.PageNumber, searchParams.PageSize);
        }

        public void UpdateHolidaySchedule(HolidaySchedule holidaySchedule)
        {
            _context.HolidaySchedules.Update(holidaySchedule);
        }
    }
}

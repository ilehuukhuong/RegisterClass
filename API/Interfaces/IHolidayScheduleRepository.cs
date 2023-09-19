using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IHolidayScheduleRepository
    {
        void AddHolidaySchedule(HolidaySchedule holidaySchedule);
        bool DeleteHolidaySchedule(int id);
        void UpdateHolidaySchedule(HolidaySchedule holidaySchedule);
        Task<HolidaySchedule> GetHolidayScheduleById(int id);
        Task<PagedList<HolidaySchedule>> GetHolidaySchedules(SearchParams searchParams);
    }
}

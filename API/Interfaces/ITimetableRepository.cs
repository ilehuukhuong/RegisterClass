using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ITimetableRepository
    {
        void AddTimetable(Timetable timetable);
        bool DeleteTimetable(Timetable timetable);
        void UpdateTimetable(Timetable timetable);
        Task<Timetable> GetTimetableById(int id);
        Task<PagedList<TimetableDto>> GetTimetables(SearchParams searchParams);
        Task<IEnumerable<StudentTimetableDto>> GetStudentTimetable(int userId);
    }
}

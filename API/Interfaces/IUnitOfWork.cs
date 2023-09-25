namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IGenderRepository GenderRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        IDepartmentFacultyRepository DepartmentFacultyRepository { get; }
        ICourseRepository CourseRepository { get; }
        IClassRepository ClassRepository { get; }
        IGradeCategoryRepository GradeCategoryRepository { get; }
        ITimetableRepository TimetableRepository { get; }
        IHolidayScheduleRepository HolidayScheduleRepository { get; }
        IGradeRepository GradeRepository { get; }
        ITuitionFeeRepository TuitionFeeRepository { get; }
        ISalaryRepository SalaryRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}

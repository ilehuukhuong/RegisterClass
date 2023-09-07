namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IGenderRepository GenderRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        IDepartmentFacultyRepository DepartmentFacultyRepository { get; }
        ICourseRepository CourseRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}

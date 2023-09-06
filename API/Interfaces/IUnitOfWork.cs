namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IGenderRepository GenderRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}

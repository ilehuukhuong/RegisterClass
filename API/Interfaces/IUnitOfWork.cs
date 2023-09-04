namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IGenderRepository GenderRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}

namespace AccountsViewer.API.Repositories.Interfaces;

public interface IUnitOfWork
{
    IAccountRepository AccountRepository { get; }
    IEntryRepository EntryRepository { get; }
    IUserRepository UserRepository { get; }
    Task Commit();
}
using AccountsViewer.API.Reporting;

namespace AccountsViewer.API.Repositories.Interfaces;

public interface IUnitOfWork
{
    AccountRepository AccountRepository { get; }
    EntryRepository EntryRepository { get; }
    UserRepository UserRepository { get; }
    StatsReporter StatsReporter { get; }
    Task Commit();
}
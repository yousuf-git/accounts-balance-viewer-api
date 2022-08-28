using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Repositories.Interfaces;

namespace AccountsViewer.API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _provider;

    private IAccountRepository? _accountRepository;
    private IEntryRepository? _entryRepository;
    private IUserRepository? _userRepository;

    public UnitOfWork(AppDbContext context, IServiceProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    private T InitService<T>(ref T member)
    {
        if (member == null)
        {
            member = _provider.GetService<T>()!;
        }

        return member;
    }

    public IAccountRepository AccountRepository => InitService(ref _accountRepository)!;
    public IEntryRepository EntryRepository => InitService(ref _entryRepository)!;
    public IUserRepository UserRepository => InitService(ref _userRepository)!;

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}
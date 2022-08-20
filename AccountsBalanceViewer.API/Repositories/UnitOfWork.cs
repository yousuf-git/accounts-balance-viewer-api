using AccountsViewer.API.Models.Contexts;

namespace AccountsViewer.API.Repositories;

public interface IUnitOfWork
{
    AccountRepository AccountRepository { get; }
    EntryRepository EntryRepository { get; }
    UserRepository UserRepository { get; }
    Task Commit();
}
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private AccountRepository? _accountRepository;
    private EntryRepository? _entryRepository;
    private UserRepository? _userRepository;
    
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    public AccountRepository AccountRepository
    {
        get => _accountRepository ??= new AccountRepository(_context);
        private set => _accountRepository = value;
    }  
    
    public EntryRepository EntryRepository
    {
        get => _entryRepository ??= new EntryRepository(_context);
        private set => _entryRepository = value;
    }

    public UserRepository UserRepository
    {
        get => _userRepository ??= new UserRepository(_context);
        private set => _userRepository = value;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}
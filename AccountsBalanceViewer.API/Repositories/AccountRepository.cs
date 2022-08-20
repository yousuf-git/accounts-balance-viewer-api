using System.Linq.Expressions;
using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Repositories;

public interface IAccountRepository : IRepository<Account>
{
} 

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    
    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Add(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public async Task<Account?> Find(long id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<IEnumerable<Account>> FindAll()
    {
        return await _context.Accounts.ToListAsync();
    }

    public IEnumerable<Account> Search(Expression<Func<Account, bool>> predicate)
    {
        return _context.Accounts.Where(predicate);
    }

    public void Delete(Account account)
    {
        _context.Accounts.Remove(account); ;
    }
}
using System.Linq.Expressions;
using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<IEnumerable<AccountDTO>> FindAllWithBalancesWithinRange(DateOnly balanceFrom, DateOnly balanceTo);
}

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task Add(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public virtual async Task<Account?> Find(long id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public virtual async Task<IEnumerable<Account>> FindAll()
    {
        return await _context.Accounts.ToListAsync();
    }

    public virtual Task<IEnumerable<AccountDTO>> FindAllWithBalancesWithinRange(DateOnly balanceFrom,
        DateOnly balanceTo)
    {
        return Task.FromResult<IEnumerable<AccountDTO>>(_context.Accounts
            .Select(account => new AccountDTO
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Entries
                    .Where(entry =>
                        DateOnly.FromDateTime(entry.Date) >= balanceFrom &&
                        DateOnly.FromDateTime(entry.Date) <= balanceTo)
                    .Sum(entry => entry.Amount)
            }));
    }

    public virtual IEnumerable<Account> Search(Expression<Func<Account, bool>> predicate)
    {
        return _context.Accounts.Where(predicate);
    }

    public virtual void Delete(Account account)
    {
        _context.Accounts.Remove(account);
        ;
    }
}
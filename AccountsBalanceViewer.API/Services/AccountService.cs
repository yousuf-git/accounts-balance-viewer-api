using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories;

namespace AccountsViewer.API.Services;

public interface IAccountService
{
    Task AddAccount(Account account);
    Task UpdateAccount(long id, Account account);
    Task DeleteAccount(long id);
    Task<List<AccountDTO>> GetAccountsWithBalancesWithinRange(DateOnly balanceFrom, DateOnly balanceTo);
}

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _uow;

    public AccountService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task AddAccount(Account account)
    {
        await _uow.AccountRepository.Add(account);
        await _uow.Commit();
    }

    public async Task<List<AccountDTO>> GetAccountsWithBalancesWithinRange(DateOnly balanceFrom, DateOnly balanceTo)
    {
        var accounts = await _uow.AccountRepository.FindAllWithBalancesWithinRange(balanceFrom, balanceTo);
        return accounts.ToList();
    }

    public async Task UpdateAccount(long id, Account account)
    {
        var a = await _uow.AccountRepository.Find(id);
        if (a == null)
        {
            throw new Exception("Account not found");
        }

        a.Name = account.Name;
        await _uow.Commit();
    }

    public async Task DeleteAccount(long id)
    {
        var account = await _uow.AccountRepository.Find(id);
        if (account == null)
        {
            throw new Exception("Account not found");
        }

        _uow.AccountRepository.Delete(account);
        await _uow.Commit();
    }
}
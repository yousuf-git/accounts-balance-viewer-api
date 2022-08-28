using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Services.Interfaces;

public interface IAccountService
{
    Task AddAccount(Account account);
    Task UpdateAccount(long id, Account account);
    Task DeleteAccount(long id);
    Task<List<AccountDTO>> GetAccountsWithBalancesWithinRange(DateOnly balanceFrom, DateOnly balanceTo);
}
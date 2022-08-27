using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Repositories.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<IEnumerable<AccountDTO>> FindAllWithBalancesWithinRange(DateOnly balanceFrom, DateOnly balanceTo);
}
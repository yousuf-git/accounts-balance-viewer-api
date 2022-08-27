using System.Linq.Expressions;
using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Models.DTOs;
using AccountsViewer.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Repositories;

public interface IStatsRepository
{
    Task<object> FindBalanceChangeByMonths(int year);
    Task<object> FindBalanceChangeByYears();
    Task<object> FindBalanceByMonths(int year);
    Task<object> FindBalanceByYears();
    object FindFirstOperationYear();
}

public class StatsRepository : IStatsRepository
{
    private readonly AppDbContext _context;

    public StatsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> FindBalanceChangeByMonths(int year)
    {
        var data = from entry in _context.Entries
            join account in _context.Accounts on entry.AccountId equals account.Id
            where entry.Date.Year == year
            group entry by new
            {
                entry.AccountId,
                AccountName = account.Name,
            }
            into g
            select new
            {
                AccountId = g.Key.AccountId,
                AccountName = g.Key.AccountName,
                Data = g.Select(entry => new { entry.Date.Month, entry.Amount })
                    .OrderBy(arg => arg.Month)
                    .GroupBy(arg => arg.Month).Select(grouping => new
                    {
                        Month = grouping.Key,
                        Change = grouping.Sum(arg => arg.Amount)
                    })
            };

        return await data.ToListAsync();
    }

    public async Task<object> FindBalanceChangeByYears()
    {
        var data = from entry in _context.Entries
            join account in _context.Accounts on entry.AccountId equals account.Id
            group entry by new
            {
                entry.AccountId,
                AccountName = account.Name,
            }
            into g
            select new
            {
                AccountId = g.Key.AccountId,
                AccountName = g.Key.AccountName,
                Data = g.Select(entry => new { entry.Date.Year, entry.Amount })
                    .OrderBy(arg => arg.Year)
                    .GroupBy(arg => arg.Year)
                    .Select(grouping => new
                    {
                        Year = grouping.Key,
                        Change = grouping.Sum(arg => arg.Amount)
                    })
            };

        return await data.ToListAsync();
    }

    public async Task<object> FindBalanceByMonths(int year)
    {
        var openingBalancesMap = new Dictionary<long, float>();
        var accountNamesMap = new Dictionary<long, string>();

        // query all accounts
        var accounts = from account in _context.Accounts
            select new { Id = account.Id, Name = account.Name };

        foreach (var account in accounts)
        {
            accountNamesMap[account.Id] = account.Name;
        }

        // query opening account balances for the year grouped by account id 
        var openingBalances = from entry in _context.Entries
            where entry.Date.Year < year
            group entry by entry.AccountId
            into g
            select new
            {
                AccountId = g.Key,
                Balance = g.Sum(entry => entry.Amount)
            };

        // map balances to account ids
        foreach (var entry in openingBalances)
        {
            openingBalancesMap.Add(entry.AccountId, entry.Balance);
        }

        // query monthly changes for the year grouped by account id 
        var monthlyChanges = from entry in _context.Entries
            where entry.Date.Year == year
            group entry by new
            {
                entry.AccountId
            }
            into g
            select new
            {
                AccountId = g.Key.AccountId,
                Data = g.Select(entry => new { entry.Date.Month, entry.Amount })
                    .GroupBy(arg => arg.Month)
                    .Select(grouping => new
                    {
                        Month = grouping.Key,
                        Change = grouping.Sum(arg => arg.Amount)
                    })
            };

        var balancesMap = new Dictionary<long, float[]>();
        foreach (var entry in monthlyChanges)
        {
            balancesMap[entry.AccountId] = Enumerable.Repeat(0f, 12).ToArray();

            foreach (var d in entry.Data)
            {
                balancesMap[entry.AccountId][d.Month - 1] += d.Change;
            }
        }

        foreach (var (accountId, balances) in balancesMap)
        {
            for (int i = 0; i < balances.Length; i++)
            {
                if (i == 0)
                {
                    var openingBalance = openingBalancesMap.ContainsKey(accountId) ? openingBalancesMap[accountId] : 0;
                    balancesMap[accountId][i] += openingBalance;
                    continue;
                }

                balancesMap[accountId][i] += balancesMap[accountId][i - 1];
            }
        }

        return balancesMap.Select(pair =>
            new
            {
                AccountId = pair.Key,
                AccountName = accountNamesMap[pair.Key],
                Data = pair.Value.Select((f, i) =>
                    new
                    {
                        Month = i + 1,
                        Balance = f
                    })
            });
    }

    public async Task<object> FindBalanceByYears()
    {
        var rangeQuery = from entry in _context.Entries
            group entry by true
            into g
            select new
            {
                Min = g.Min(entry => entry.Date.Year),
                Max = g.Max(entry => entry.Date.Year),
            };

        var range = await rangeQuery.SingleAsync();

        var accountsQuery = from entry in _context.Entries
            join account in _context.Accounts on entry.AccountId equals account.Id
            group entry by new
            {
                entry.AccountId, AccountName = account.Name
            }
            into g
            select new
            {
                g.Key.AccountId,
                g.Key.AccountName,
                Data = g.Select(entry => new { entry.Date.Year, entry.Amount })
                    .GroupBy(arg => new { arg.Year })
                    .Select(arg => new
                    {
                        Year = arg.Key.Year,
                        Amount = arg.Sum(arg1 => arg1.Amount)
                    })
            };

        var balancesMap = new Dictionary<long, float[]>();
        var accountNamesMap = new Dictionary<long, string>();

        foreach (var entry in accountsQuery)
        {
            // cache account name mapped to account id
            accountNamesMap[entry.AccountId] = entry.AccountName;

            balancesMap[entry.AccountId] = Enumerable.Repeat(0f, range.Max - range.Min + 1).ToArray();

            int i = -1;
            foreach (var d in entry.Data)
            {
                var offset = (range.Max - range.Min) + i;
                balancesMap[entry.AccountId][offset] += d.Amount;
                i++;
            }
        }

        foreach (var (accountId, balances) in balancesMap)
        {
            for (int i = 1; i < balances.Length; i++)
            {
                balancesMap[accountId][i] += balancesMap[accountId][i - 1];
            }
        }

        return balancesMap.Select(pair =>
            new
            {
                AccountId = pair.Key,
                AccountName = accountNamesMap[pair.Key],
                Data = pair.Value.Select((f, i) =>
                    new
                    {
                        Year = range.Min + i,
                        Balance = f
                    })
            });
    }

    public object FindFirstOperationYear()
    {
        return _context.Entries.Min(entry => entry.Date.Year);
    }
}
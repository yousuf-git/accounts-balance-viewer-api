using AccountsViewer.API.Models.DTOs;

namespace AccountsViewer.API.Reporting.Interfaces;

public interface IStatsReporter
{
    Task<List<BalanceChangeByMonthsDTO>> FindBalanceChangeByMonths(int year);
    Task<List<BalanceChangeByYearsDTO>> FindBalanceChangeByYears();
    Task<List<BalanceByMonthsDTO>> FindBalanceByMonths(int year);
    Task<List<BalanceByYearsDTO>> FindBalanceByYears();
    int FindFirstOperationYear();
}
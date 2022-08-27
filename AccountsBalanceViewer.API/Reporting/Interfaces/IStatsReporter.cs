namespace AccountsViewer.API.Reporting.Interfaces;

public interface IStatsReporter
{
    Task<object> FindBalanceChangeByMonths(int year);
    Task<object> FindBalanceChangeByYears();
    Task<object> FindBalanceByMonths(int year);
    Task<object> FindBalanceByYears();
    int FindFirstOperationYear();
}
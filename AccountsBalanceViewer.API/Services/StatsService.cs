using AccountsViewer.API.Repositories;

namespace AccountsViewer.API.Services;

public interface IStatsService
{
    Task<object> FindBalanceChangeByMonths(int year);
    Task<object> FindBalanceChangeByYears();
    Task<object> FindBalanceByMonths(int year);
    Task<object> FindBalanceByYears();
    object FindFirstOperationYear();
}

public class StatsService : IStatsService
{
    private readonly IUnitOfWork _uow;

    public StatsService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<object> FindBalanceChangeByMonths(int year)
    {
        return await _uow.StatsRepository.FindBalanceChangeByMonths(year);
    }

    public async Task<object> FindBalanceChangeByYears()
    {
        return await _uow.StatsRepository.FindBalanceChangeByYears();
    }

    public async Task<object> FindBalanceByMonths(int year)
    {
        return await _uow.StatsRepository.FindBalanceByMonths(year);
    }

    public async Task<object> FindBalanceByYears()
    {
        return await _uow.StatsRepository.FindBalanceByYears();
    }

    public object FindFirstOperationYear()
    {
        return _uow.StatsRepository.FindFirstOperationYear();
    }
}
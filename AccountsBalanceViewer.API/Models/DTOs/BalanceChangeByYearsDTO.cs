namespace AccountsViewer.API.Models.DTOs;

public class BalanceChangeByYearsDTO
{
    public long AccountId { get; set; }
    public string? AccountName { get; set; }
    public List<BalanceChangeByYearsStat> Data { get; set; } = new();

    public class BalanceChangeByYearsStat
    {
        public int Year { get; set; }
        public float Change { get; set; }
    }
}
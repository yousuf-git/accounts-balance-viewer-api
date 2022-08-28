namespace AccountsViewer.API.Models.DTOs;

public class BalanceChangeByMonthsDTO
{
    public long AccountId { get; set; }
    public string? AccountName { get; set; }
    public List<BalanceChangeByMonthsStat> Data { get; set; } = new();

    public class BalanceChangeByMonthsStat
    {
        public int Month { get; set; }
        public float Change { get; set; }
    }
}
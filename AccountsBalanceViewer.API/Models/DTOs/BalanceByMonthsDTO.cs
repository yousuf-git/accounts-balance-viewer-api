namespace AccountsViewer.API.Models.DTOs;

public class BalanceByMonthsDTO
{
    public long AccountId { get; set; }
    public string? AccountName { get; set; }
    public List<BalanceByMonthsStat> Data { get; set; } = new();

    public class BalanceByMonthsStat
    {
        public int Month { get; set; }
        public float Balance { get; set; }
    }
}
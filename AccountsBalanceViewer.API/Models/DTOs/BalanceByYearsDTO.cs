namespace AccountsViewer.API.Models.DTOs;

public class BalanceByYearsDTO
{
    public long AccountId { get; set; }
    public string? AccountName { get; set; }
    public List<BalanceByYearsStat> Data { get; set; } = new();

    public class BalanceByYearsStat
    {
        public int Year { get; set; }
        public float Balance { get; set; }
    }
}
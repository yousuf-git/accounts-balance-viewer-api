namespace AccountsViewer.API.Models.Entities;

public class Entry
{
    public long Id { get; set; }
    public long Amount { get; set; }
    public DateTime Date { get; set; }
    
    public long AccountId { get; set; }
    public Account Account { get; set; } = new();
    public long CreatedBy { get; set; }
    public User User { get; set; } = new();
}
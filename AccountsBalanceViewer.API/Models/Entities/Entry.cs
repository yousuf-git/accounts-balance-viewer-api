namespace AccountsViewer.API.Models.Entities;

public class Entry
{
    public long Id { get; set; }
    public float Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
    
    public long AccountId { get; set; }
    public Account? Account { get; set; }
    public long CreatedBy { get; set; }
    public User? User { get; set; }
}
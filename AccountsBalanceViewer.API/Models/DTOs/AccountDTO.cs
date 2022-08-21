namespace AccountsViewer.API.Models.DTOs;

public class AccountDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public float Balance { get; set; }
}
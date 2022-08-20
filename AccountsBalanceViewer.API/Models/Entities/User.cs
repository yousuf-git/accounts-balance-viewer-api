using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Models.Entities;

[Index(nameof(Username))]
[Index(nameof(Email))]
public class User
{
    public static string ROLE_ADMIN = "admin";
    public static string ROLE_USER = "user";
    
    public long Id { get; set; }
    public string Username { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
    public List<Entry> Entries { get; set; } = new();
}
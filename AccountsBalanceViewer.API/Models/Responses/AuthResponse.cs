namespace AccountsViewer.API.Models.Responses;

public class AuthResponse
{
    public long Id { get; set; }
    public string Username { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public string Token { get; set; } = "";
    public long Expires { get; set; }
}
using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Models.DTOs;

public class UserDTO
{
    public long Id { get; set; }
    public string Username { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    
    public UserDTO() {}

    public UserDTO(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Name = user.Name;
        Email = user.Email;
        Role = user.Role;
    }
}
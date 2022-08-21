using System.ComponentModel.DataAnnotations;

namespace AccountsViewer.API.Models.Requests;

public class AuthRequest
{
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}
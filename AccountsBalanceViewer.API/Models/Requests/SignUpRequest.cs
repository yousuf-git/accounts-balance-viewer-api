using System.ComponentModel.DataAnnotations;

namespace AccountsViewer.API.Models.Requests;

public class SignUpRequest
{
    [Required] public string? Name { get; set; }
    [Required] public string? Email { get; set; }
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}
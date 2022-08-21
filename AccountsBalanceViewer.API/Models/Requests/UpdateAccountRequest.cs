using System.ComponentModel.DataAnnotations;

namespace AccountsViewer.API.Models.Requests;

public class UpdateAccountRequest
{
    [Required] public string? Name { get; set; }
}
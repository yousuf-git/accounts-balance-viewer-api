using System.ComponentModel.DataAnnotations;

namespace AccountsViewer.API.Models.Requests;

public class AddAccountRequest
{
    [Required] public string? Name { get; set; }
}
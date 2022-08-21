using System.ComponentModel.DataAnnotations;

namespace AccountsViewer.API.Models.Requests;

public class AddEntryRequest
{
    [Required] public long AccountId { get; set; }
    [Required] public float Amount { get; set; }
    [Required] public DateTime Date { get; set; } = DateTime.Now.ToUniversalTime();
}
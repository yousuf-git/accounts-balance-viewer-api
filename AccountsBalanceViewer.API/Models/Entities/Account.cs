using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Models.Entities;

[Index(nameof(Name))]
public class Account
{
    public long Id { get; set; }
    public string Name { get; set; } = "";

    public List<Entry> Entries { get; set; } = new();
}
using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Services.Interfaces;

public interface IEntryService
{
    Task AddEntries(IEnumerable<Entry> entries);
}
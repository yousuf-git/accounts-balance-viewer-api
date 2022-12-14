using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Repositories.Interfaces;

public interface IEntryRepository : IRepository<Entry>
{
    Task AddRange(IEnumerable<Entry> entries);
}
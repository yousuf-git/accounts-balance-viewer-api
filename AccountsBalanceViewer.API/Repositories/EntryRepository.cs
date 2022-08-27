using System.Linq.Expressions;
using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Models.Entities;
using AccountsViewer.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountsViewer.API.Repositories;

public class EntryRepository : IEntryRepository
{
    private readonly AppDbContext _context;

    public EntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task Add(Entry entry)
    {
        await _context.Entries.AddAsync(entry);
    }

    public virtual async Task AddRange(IEnumerable<Entry> entries)
    {
        await _context.Entries.AddRangeAsync(entries);
    }

    public virtual async Task<Entry?> Find(long id)
    {
        return await _context.Entries.FindAsync(id);
    }

    public virtual async Task<IEnumerable<Entry>> FindAll()
    {
        return await _context.Entries.ToListAsync();
    }

    public virtual IEnumerable<Entry> Search(Expression<Func<Entry, bool>> predicate)
    {
        return _context.Entries.Where(predicate);
    }

    public virtual void Delete(Entry entry)
    {
        _context.Entries.Remove(entry);
    }
}
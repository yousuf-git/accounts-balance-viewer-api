using System.Linq.Expressions;
using AccountsViewer.API.Models.Contexts;
using AccountsViewer.API.Models.Entities;

namespace AccountsViewer.API.Repositories;

public interface IUserRepository : IRepository<User>
{
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> Find(long id)
    {
        return await _context.Users.FindAsync(id);
    }

    public Task<IEnumerable<User>> FindAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> Search(Expression<Func<User, bool>> predicate)
    {
        return _context.Users.Where(predicate);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }
}
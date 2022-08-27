using System.Linq.Expressions;

namespace AccountsViewer.API.Repositories.Interfaces;

public interface IRepository<T>
{
    Task Add(T entity);
    Task<T?> Find(long id);
    Task<IEnumerable<T>> FindAll();
    IEnumerable<T> Search(Expression<Func<T, bool>> predicate);
    void Delete(T entity);
}
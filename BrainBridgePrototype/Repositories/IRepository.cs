using System.Linq.Expressions;

namespace BrainBridgePrototype.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    }
}

using System.Linq.Expressions;
using System.Security.Cryptography;

namespace APP.Repositories
{
	public interface IGenericRepository<T, TId> where T : class where TId : struct
	{
		IQueryable<T> GetAll();
		IQueryable<T> Where(Expression<Func<T, bool>> predicate);
		Task<bool> AnyAsync(TId id);
		ValueTask<T?> GetByIdAsync(int id);
		ValueTask AddAsync(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APP.Repositories
{
	public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
	{
		protected AppDbContext Context = context;

		private readonly DbSet<T> _dbset = context.Set<T>();
		public async ValueTask AddAsync(T entity) => await _dbset.AddAsync(entity);


		public void Delete(T entity) => _dbset.Remove(entity);
		

		public IQueryable<T> GetAll() => _dbset.AsQueryable().AsNoTracking();


		public ValueTask<T?> GetByIdAsync(int id) => _dbset.FindAsync(id);
		

		public void Update(T entity) => _dbset.Update(entity);
		

		public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbset.Where(predicate).AsNoTracking();
		
	}
}

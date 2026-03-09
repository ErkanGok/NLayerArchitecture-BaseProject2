using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Repositories.Categories
{
	public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
	{
		public IQueryable<Category?> GetCategoryWithProducts()
		{
			return context.Categories.Include(x => x.Products).AsQueryable();
		}

		public Task<Category?> GetCategoryWithProductsAsync(int id)
		{
			//eager loading
			return context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.ID == id);
			
		}
	}
}

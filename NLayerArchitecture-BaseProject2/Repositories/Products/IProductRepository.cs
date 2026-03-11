using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Repositories.Products
{
	public interface IProductRepository:IGenericRepository<Product,int>
	{
		public Task<List<Product>> GetTopPriceProductsAsync(int count);
	}
}

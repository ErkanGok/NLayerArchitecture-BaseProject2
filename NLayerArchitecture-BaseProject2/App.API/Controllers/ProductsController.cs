using App.Services;
using App.Services.Filters;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using APP.Repositories.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.API.Controllers
{
	
	public class ProductsController(IProductService productService) : CustomBaseController
	{
		[HttpGet]
		public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());

		[HttpGet("{pageNumber:int}/{pageSize:int}")]
		public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) => CreateActionResult(await productService.GetPagedAllListAsync(pageNumber,pageSize));

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));

		[HttpPost]
		public async Task<IActionResult> Create(CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));

		[ServiceFilter(typeof(NotFoundFilter<Product, int>))]
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));


		[HttpPatch("stock")]
		public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) => CreateActionResult(await productService.UpdateStockAsync(request));

		[ServiceFilter(typeof(NotFoundFilter<Product,int>))]
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
		
	}
}

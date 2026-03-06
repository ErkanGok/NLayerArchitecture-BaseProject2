using APP.Repositories;
using APP.Repositories.Products;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products
{
	public class ProductService(IProductRepository productRepository, IUnitofWork unitofWork/*, IValidator<CreateProductRequest> createProductRequestValidator*/) : IProductService
	{
		public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
		{
			var products = await productRepository.GetTopPriceProductsAsync(count);

			var ProductsAsDto = products.Select(p => new ProductDto(p.ID, p.Name, p.Price, p.Stock)).ToList();

			return new ServiceResult<List<ProductDto>>()
			{
				Data = ProductsAsDto
			};
		}

		public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
		{
			var products = await productRepository.GetAll().ToListAsync();
			var productsAsDto = products.Select(p => new ProductDto(p.ID, p.Name, p.Price, p.Stock)).ToList();

			return ServiceResult<List<ProductDto>>.Success(productsAsDto);
		}

		public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
		{

			var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			var productsAsDto = products.Select(p => new ProductDto(p.ID, p.Name, p.Price, p.Stock)).ToList();

			return ServiceResult<List<ProductDto>>.Success(productsAsDto);
		}


		public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
		{
			var product = await productRepository.GetByIdAsync(id);

			if (product is null)
			{
				ServiceResult<ProductDto>.Fail("Product Not Found", HttpStatusCode.NotFound);
			}

			var ProductsAsDto = new ProductDto(product!.ID, product.Name, product.Price, product.Stock);

			return ServiceResult<ProductDto>.Success(ProductsAsDto)!;
		}

		public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
		{
			// 2. way async manuel service business check
			var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();

			if (anyProduct)
			{
				return ServiceResult<CreateProductResponse>.Fail("Ürün İsmi Veritabanında Bulunmaktadır.", HttpStatusCode.BadRequest);
			}

			#region 3. yol manuel async fluent validation validate
			//var validationResult = await createProductRequestValidator.ValidateAsync(request);

			//if (!validationResult.IsValid) 
			//{
			//	return ServiceResult<CreateProductResponse>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
			//}
			#endregion

			var product = new Product()
			{
				Name = request.Name,
				Price = request.Price,
				Stock = request.Stock,
			};
			await productRepository.AddAsync(product);
			await unitofWork.SaveChangesAsync();
			return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.ID),$"api/products/{product.ID}");
		}

		public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
		{
			var product = await productRepository.GetByIdAsync(id);

			if (product is null)
			{
				return ServiceResult.Fail("Product Not Found", HttpStatusCode.NotFound);
			}

			product.Name = request.Name;
			product.Price = request.Price;
			product.Stock = request.Stock;

			productRepository.Update(product);
			await unitofWork.SaveChangesAsync();

			return ServiceResult.Success(HttpStatusCode.NoContent);

		}

		public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
		{
			var product = await productRepository.GetByIdAsync(request.ProductId);

			if(product is null)
			{
				return ServiceResult.Fail("Product Not Found", HttpStatusCode.NotFound);
			}

			product.Stock = request.Quantity;
			productRepository.Update(product);
			await unitofWork.SaveChangesAsync();

			return ServiceResult.Success(HttpStatusCode.NoContent);
		}

		public async Task<ServiceResult> DeleteAsync(int id)
		{
			var product = await productRepository.GetByIdAsync(id);

			if (product is null)
			{
				return ServiceResult.Fail("Product Not Found", HttpStatusCode.NotFound);
			}

			productRepository.Delete(product);
			await unitofWork.SaveChangesAsync();
			return ServiceResult.Success(HttpStatusCode.NoContent);

		}
	}
}

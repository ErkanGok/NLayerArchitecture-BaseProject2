using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using APP.Repositories;
using APP.Repositories.Categories;
using APP.Repositories.Products;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;

namespace App.Services.Categories;

public class CategoryService(ICategoryRepository categoryRepository, IUnitofWork unitofWork, IMapper mapper) : ICategoryService
{
	public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId)
	{
		var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);

		if(category is null)
		{
			return ServiceResult<CategoryWithProductsDto>.Fail("Kategori Bulunamadı", HttpStatusCode.NotFound);
		}
		var categoryAsDto = mapper.Map<CategoryWithProductsDto>(category);

		return ServiceResult<CategoryWithProductsDto>.Success(categoryAsDto);
	}

	public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync()
	{
		var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();

		
		var categoryAsDto = mapper.Map<List<CategoryWithProductsDto>>(category);

		return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryAsDto);
	}

	public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
	{
		var isCategoryNameExist = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();

		if (isCategoryNameExist)
		{
			return ServiceResult<int>.Fail("Kategori İsmi Veritabanında Bulunmaktadır.", HttpStatusCode.BadRequest);
		}

		var newCategory = mapper.Map<Category>(request);

		await categoryRepository.AddAsync(newCategory);
		await unitofWork.SaveChangesAsync();

		return ServiceResult<int>.SuccessAsCreated(newCategory.Id,$"api/categories/{newCategory.Id}");

	}

	public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
	{
		

		var isProductNameExist = await categoryRepository.Where(x => x.Name == request.Name && x.Id != id).AnyAsync();

		if (isProductNameExist)
		{
			return ServiceResult.Fail("Kategori İsmi Veritabanında Bulunmaktadır.", HttpStatusCode.BadRequest);
		}

		var category = mapper.Map<Category>(request);
		category.Id = id;

		categoryRepository.Update(category);
		await unitofWork.SaveChangesAsync();

		return ServiceResult.Success(HttpStatusCode.NoContent);

	}

	public async Task<ServiceResult> DeleteAsync(int id)
	{
		var category = await categoryRepository.GetByIdAsync(id);

		
		categoryRepository.Delete(category!);
		await unitofWork.SaveChangesAsync();
		return ServiceResult.Success(HttpStatusCode.NoContent);

	}

	public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
	{
		var categories = await categoryRepository.GetAll().ToListAsync();
		var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);
		#region Manuel mapping
		//var productsAsDto = products.Select(p => new ProductDto(p.ID, p.Name, p.Price, p.Stock)).ToList();
		#endregion

		return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
	}

	public async Task<ServiceResult<CategoryDto?>> GetByIdAsync(int id)
	{
		var categories = await categoryRepository.GetByIdAsync(id);

		if (categories is null)
		{
			return ServiceResult<CategoryDto?>.Fail("Category Not Found", HttpStatusCode.NotFound);
		}

		var categoriesAsDto = mapper.Map<CategoryDto>(categories);
		#region Manuel Mapping
		//var productsAsDto = new ProductDto(product!.ID, product.Name, product.Price, product.Stock);
		#endregion

		return ServiceResult<CategoryDto>.Success(categoriesAsDto)!;
	}
}

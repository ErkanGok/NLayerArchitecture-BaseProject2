using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using App.Services.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using APP.Repositories.Categories;
using APP.Repositories.Products;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Categories
{
	public class CategoryMappingProfile : Profile
	{
		public CategoryMappingProfile()
		{
			CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

			CreateMap<CategoryDto, Category>().ReverseMap();

			CreateMap<Category,CategoryWithProductsDto>().ReverseMap();

			CreateMap<CreateCategoryRequest, Category>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));

			CreateMap<UpdateCategoryRequest, Category>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
		}
	}
}

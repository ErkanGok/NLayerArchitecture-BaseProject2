using App.Services.Products.Create;
using App.Services.Products.Update;
using APP.Repositories.Products;
using AutoMapper;
using System.Globalization;

namespace App.Services.Products
{
	public class ProductMappingProfile : Profile
	{
		public ProductMappingProfile()
		{
			CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

			CreateMap<Product, ProductDto>().ReverseMap();

			CreateMap<CreateProductRequest, Product>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));

			CreateMap<UpdateProductRequest, Product>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToLowerInvariant()));
		}
	}
}

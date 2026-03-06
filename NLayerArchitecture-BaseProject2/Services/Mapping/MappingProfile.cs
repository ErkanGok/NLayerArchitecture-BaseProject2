using App.Services.Products;
using APP.Repositories.Products;
using AutoMapper;

namespace App.Services.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Product, ProductDto>().ReverseMap();
		}
	}
}

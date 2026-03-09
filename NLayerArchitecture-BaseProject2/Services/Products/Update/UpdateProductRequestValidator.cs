using App.Services.Products.Create;
using APP.Repositories.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products.Update
{
	public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
	{
		private readonly IProductRepository _productRepository;
		public UpdateProductRequestValidator(IProductRepository productRepository)
		{
			_productRepository = productRepository;
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Ürün İsmi Gereklidir.")
				.Length(3, 10).WithMessage("Ürün İsmi 3 ile 10 Karakter Arasında Olmalıdır.");
		

			RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("Ürün Fiyatı 0'dan Büyük Olmalıdır.");

			RuleFor(x => x.Stock)
				.InclusiveBetween(1, 100).WithMessage("Stok Adeti 1 ile 100 arasında olmalıdır.");

			RuleFor(x => x.CategoryId)
				.GreaterThan(0).WithMessage("Ürün Kategori Değeri 0'dan Büyük Olmalıdır.");
		}
	}
}

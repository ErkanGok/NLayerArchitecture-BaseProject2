using APP.Repositories.Products;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products.Create
{
	public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
	{
		private readonly IProductRepository _productRepository;


		public CreateProductRequestValidator(IProductRepository productRepository)
		{
			_productRepository = productRepository;
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Ürün İsmi Gereklidir.")
				.Length(3, 10).WithMessage("Ürün İsmi 3 ile 10 Karakter Arasında Olmalıdır.");
			//.Must(MustUniqueProductName).WithMessage("Ürün İsmi Veritabanında Bulunmaktadır."); //1.yol
			//.MustAsync(MustUniqueProductNameAsync).WithMessage("Ürün İsmi Veritabanında Bulunmaktadır."); //.net default pipeline'da çalışmıyor burası 3.yol Çalışması için ServiceExtension'da services.AddFluentValidationAutoValidation(); kapatacağız ve ProductService interface geçicez ilgili controller'a yazıcaz

			RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("Ürün Fiyatı 0'dan Büyük Olmalıdır.");

			RuleFor(x => x.Stock)
				.InclusiveBetween(1, 100).WithMessage("Stok Adeti 1 ile 100 arasında olmalıdır.");
		}

		#region 1. way sync validation
		//public bool MustUniqueProductName(string name) => !_productRepository.Where(x => x.Name == name).Any();
		#endregion


		#region 2. way async validaiton(active)
		//2. yol productservice içersinde ilgili controller'a yazıldı.
		#endregion

		#region 3. way async validation
		//public async Task<bool> MustUniqueProductNameAsync(string name, CancellationToken cancellationToken) => !await _productRepository.Where(x => x.Name == name).AnyAsync(cancellationToken);
		#endregion






	}
}

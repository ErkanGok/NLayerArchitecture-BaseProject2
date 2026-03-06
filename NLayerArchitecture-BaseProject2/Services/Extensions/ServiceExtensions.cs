using App.Services.ExceptionHandlers;
using App.Services.Products;
using APP.Repositories;
using APP.Repositories.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
		{
			
			services.AddScoped<IProductService, ProductService>();

			services.AddFluentValidationAutoValidation(); // burası açık olursa asenkron validation çalışmaz, Eğer bunu kaldırırsak Product service'e geçmemiz lazım (3. yol)
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			//Exceptionhandlers eklediğimiz sıra önemli
			services.AddExceptionHandler<CriticalExceptionHandler>();
			services.AddExceptionHandler<GlobalExceptionHandler>();
			
			return services;
		}
	}
}

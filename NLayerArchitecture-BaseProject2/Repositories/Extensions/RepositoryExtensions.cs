using APP.Repositories.Categories;
using APP.Repositories.Interceptors;
using APP.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APP.Repositories.Extensions
{
	public static class RepositoryExtensions
	{
		public static IServiceCollection AddRepositories(this IServiceCollection services ,IConfiguration configuration)
		{
			services.AddDbContext<AppDbContext>(options =>
			{
				var connectionStrings = configuration.GetSection(ConnectionStringOption.Key).Get<ConnectionStringOption>();

				options.UseSqlServer(connectionStrings!.SqlServer,sqlServerOptionsAction => 
				{
					sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
				});
				options.AddInterceptors(new AuditDbContextInterceptor());
			});
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IUnitofWork, UnitofWork>();
			services.AddScoped(typeof(IGenericRepository<,>),typeof(GenericRepository<,>));
			return services;
		}
	}
}

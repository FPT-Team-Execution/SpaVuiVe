using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Services;
using SkincareProductSalesSystem.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Api.Configs
{
	public static class DependencyInjection
    {
		public static void AddInfrastructure(this IServiceCollection services)
		{
			services.AddApiDependencyInjection();
			services.AddServicesDependencyInjection();
		}
		public static void AddApiDependencyInjection(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IPromotionService, PromotionService>();
			services.AddScoped<IPromotionUsageService, PromotionUsageService>();
		}

		public static void AddServicesDependencyInjection(this IServiceCollection services)
		{
			services.AddScoped<UnitOfWork>();
			services.AddScoped<JwtHelper>();

			var config = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();
			services.AddSingleton<IConfiguration>(config);
		}
	}
}

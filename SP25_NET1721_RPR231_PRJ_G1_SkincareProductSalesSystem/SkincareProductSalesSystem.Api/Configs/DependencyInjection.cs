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
using SkincareProductSalesSystem.Services;
using SkincareProductSalesSystem.Services.ExtendServices;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Repositories;

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
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<ICacheService, CacheService>();
            services.AddDbContext<SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<BrandRepository>();
            services.AddScoped<OrderDetailRepository>();
            services.AddScoped<PaymentMethodRepository>();
            services.AddScoped<PaymentRepository>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderDetailServices, OrderDetailServices>();
            services.AddScoped<IBrandService, BrandServices>();
            services.AddScoped<IPaymentServices, PaymentServices>();
            services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
            services.AddScoped<ISkinTestService, SkinTestService>();
            //services.AddScoped<ISkinTypeService, SkinTypeService>();
            services.AddScoped<IChatBotService, ChatBotService>();
			services.AddScoped<IPromotionService, PromotionService>();
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

namespace SkincareProductSalesSystem.Api.Configs
{
	public class GoogleConfiguration
	{
		public static void AddConfiguration(WebApplicationBuilder builder)
		{
			AddGoogleAuth(builder);
		}

		public static void AddGoogleAuth(WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication()
			.AddGoogle(options =>
			{
				options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
				options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
				options.CallbackPath = "/signin-google";
			});

			builder.Services.AddControllersWithViews();
		}
	}
}

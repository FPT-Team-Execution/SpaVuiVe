using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SkincareSession"; 
    options.IdleTimeout = TimeSpan.FromMinutes(30);  
    options.Cookie.IsEssential = true; 
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ApiClient>();
builder.Services.AddSingleton(typeof(GrpcClient<>), typeof(GrpcClient<>));
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});
builder.Services.AddHttpClient();
// Add authentication with cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = Const.ACCESS_TOKEN_COOKIE;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true;
    });
//* Add after login
//Response.Cookies.Append("AccessToken", tokenValue, new CookieOptions
//{
//    HttpOnly = true,
//    Secure = true,
//    SameSite = SameSiteMode.Strict,
//    // Set expiration as needed
//    Expires = DateTime.UtcNow.AddHours(1)
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

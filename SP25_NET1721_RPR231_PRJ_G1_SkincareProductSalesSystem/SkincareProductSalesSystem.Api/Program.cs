using AutoMapper;
using SkincareProductSalesSystem.Api.Configs;
using SkincareProductSalesSystem.Services.Configs;

using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Repositories;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<BrandRepository>();
builder.Services.AddScoped<OrderDetailRepository>();
builder.Services.AddScoped<PaymentMethodRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IOrderDetailServices, OrderDetailServices>();
builder.Services.AddScoped<IBrandService, BrandServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<IPaymentMethodServices, PaymentMethodServices>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig).Assembly);

DependencyInjection.AddInfrastructure(builder.Services);
JwtConfiguration.ConfigureJwt(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

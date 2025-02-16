using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Protos;
using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace SkincareProductSalesSystem.Services
{
    public class ProductGrpcService : ProductGrpcProto.ProductGrpcProtoBase
    {
        private Expression<Func<T, bool>> CombineExpressions<T>(
    Expression<Func<T, bool>> first,
    Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var parameter = Expression.Parameter(typeof(T));

            var combined = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        private readonly UnitOfWork _unitOfWork;

        public ProductGrpcService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public override async Task<ProductListResponseProto> GetAllProducts(GetAllProductQueryProto query, ServerCallContext context)
        {
            Expression<Func<Product, bool>> predicate = x => true;


            if (!query.Category.IsNullOrEmpty())
            {
                Expression<Func<Product, bool>> categoryFilter = x => x.Category.Name == query.Category;
                predicate = CombineExpressions(predicate, categoryFilter);
            }

            if (!string.IsNullOrEmpty(query.FilterBy) && !string.IsNullOrEmpty(query.FilterQuery))
            {
                Expression<Func<Product, bool>>? filterExpression = query.FilterBy.ToLower() switch
                {
                    "name" => x => x.Name.Contains(query.FilterQuery),
                    _ => null
                };

                if (filterExpression != null)
                {
                    predicate = CombineExpressions(predicate, filterExpression);
                }
            }

            Func<IQueryable<Product>, IOrderedQueryable<Product>> orderBy = null;
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                orderBy = query.IsAsc
                    ? q => q.OrderBy(x => EF.Property<Product>(x, query.SortBy))
                    : q => q.OrderByDescending(x => EF.Property<Product>(x, query.SortBy));
            }


            var products = await _unitOfWork.ProductRepository.GetPagingListAsync(
                predicate: predicate,
                orderBy: orderBy,
                page: query.Page,
                size: query.Size,
                include:
                x => x.Include(x => x.Category).Include(x => x.Brand)
            );

            var productListResponseProto = new ProductListResponseProto()
            {
                Status = 200,
                Message = "Thành Công",
                Data = new PaginateProto()
                {
                    Page = products.Page,
                    Size = products.Size,
                    Total = products.Total,
                    TotalPages = products.TotalPages,
                }
            };

            foreach (var product in products.Items)
            {
                productListResponseProto.Data.Items.Add(new ProductProto()
                {
                    BrandId = product.BrandId,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                    Ingredients = product.Ingredients,
                    StockQuantity = product.StockQuantity ??= 0,
                    IsActive = product.IsActive ??= false,
                    Name = product.Name,
                    Price  = Convert.ToDouble(product.Price),
                    ProductId = product.ProductId,
                });
            }

            return productListResponseProto;
        }


    }
}

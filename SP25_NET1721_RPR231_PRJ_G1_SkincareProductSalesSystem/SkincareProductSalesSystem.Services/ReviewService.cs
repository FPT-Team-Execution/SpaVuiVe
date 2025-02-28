using SkincareProductSalesSystem.Repositories;
using SkincareProductSalesSystem.Repositories.Models;
using SkincareProductSalesSystem.Services.Base;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.Services
{
    public class CreateReviewRequest
    {
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool? IsVisible { get; set; }
    }

    public class UpdateReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsVisible { get; set; }
    }

    public interface IReviewService
    {
        Task<IServiceResult> GetAllAsync(int page, int size);
        Task<IServiceResult> GetAsync(string id);
        Task<IServiceResult> GetByProductIdAsync(string productId, int page, int size);
        Task<IServiceResult> GetByCustomerIdAsync(string customerId, int page, int size);
        Task<IServiceResult> Create(CreateReviewRequest request);
        Task<IServiceResult> Update(string id, UpdateReviewRequest request);
        Task<IServiceResult> Delete(string id);
    }

    public class ReviewService : IReviewService
    {
        private UnitOfWork _unitOfWork;

        public ReviewService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> Create(CreateReviewRequest request)
        {
            var review = new Review
            {
                ReviewId = Guid.NewGuid().ToString(),
                ProductId = request.ProductId,
                CustomerId = request.CustomerId,
                Rating = request.Rating,
                Comment = request.Comment,
                ImageUrl = request.ImageUrl,
                IsVerified = request.IsVerified,
                PurchaseDate = request.PurchaseDate,
                IsVisible = request.IsVisible ?? true,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.ReviewRepository.CreateAsync(review);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = review,
            };
        }

        public async Task<IServiceResult> Delete(string id)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
            if (review == null) return new ServiceResult(404, "Không tìm thấy");

            await _unitOfWork.ReviewRepository.RemoveAsync(review);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = review
            };
        }

        public async Task<IServiceResult> GetAllAsync(int page, int size)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetPagingListAsync(
                include: q => q.Include(r => r.Customer).Include(r => r.Product),
                page: page,
                size: size
            );

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = reviews
            };
        }

        public async Task<IServiceResult> GetAsync(string id)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
            if (review == null) return new ServiceResult(404, "Không tìm thấy");

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = review
            };
        }

        public async Task<IServiceResult> GetByProductIdAsync(string productId, int page, int size)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetPagingListAsync(
                predicate: r => r.ProductId == productId,
                include: q => q.Include(r => r.Customer),
                orderBy: q => q.OrderByDescending(r => r.CreatedAt),
                page: page,
                size: size
            );

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = reviews
            };
        }

        public async Task<IServiceResult> GetByCustomerIdAsync(string customerId, int page, int size)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetPagingListAsync(
                predicate: r => r.CustomerId == customerId,
                include: q => q.Include(r => r.Product),
                orderBy: q => q.OrderByDescending(r => r.CreatedAt),
                page: page,
                size: size
            );

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = reviews
            };
        }

        public async Task<IServiceResult> Update(string id, UpdateReviewRequest request)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);

            if (review == null) return new ServiceResult(404, "Không tìm thấy");

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.ImageUrl = request.ImageUrl;
            review.IsVerified = request.IsVerified;
            review.IsVisible = request.IsVisible;

            await _unitOfWork.ReviewRepository.UpdateAsync(review);

            return new ServiceResult
            {
                Status = 200,
                Message = "Thành công",
                Data = review
            };
        }
    }
}


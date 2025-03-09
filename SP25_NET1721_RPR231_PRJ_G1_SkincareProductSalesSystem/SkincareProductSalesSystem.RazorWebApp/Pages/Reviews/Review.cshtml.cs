using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.Reviews
{
    public class ReviewModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public ReviewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty(SupportsGet = true)]
        public string ProductId { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
        public int TotalReviews { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingCounts { get; set; } = new Dictionary<int, int>();

        [BindProperty]
        public CreateReviewModel NewReview { get; set; } = new CreateReviewModel();

        [BindProperty]
        public EditReviewModel EditReview { get; set; } = new EditReviewModel();

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(ProductId))
            {
                return RedirectToPage("/Index");
            }

            await LoadReviews();
            return Page();
        }

        private async Task LoadReviews()
        {
            try
            {
                var reviewsResponse = await _apiClient.GetAsync($"/reviews/product/{ProductId}?page=1&size=10");

                if (reviewsResponse?.Status == 200 && reviewsResponse.Data != null)
                {
                    Console.WriteLine($"API Response: {reviewsResponse.Data}");

                    var responseData = reviewsResponse.Data.ToString();
                    var reviewsData = JsonConvert.DeserializeObject<PaginatedResult<ReviewDto>>(responseData);

                    if (reviewsData != null)
                    {
                        TotalReviews = reviewsData.Total;
                        TotalPages = reviewsData.TotalPages;
                        Reviews.Clear();

                        if (reviewsData.Items != null && reviewsData.Items.Any())
                        {
                            CalculateRatingStatistics(reviewsData.Items);

                            foreach (var review in reviewsData.Items)
                            {
                                var reviewViewModel = new ReviewViewModel
                                {
                                    ReviewId = review.ReviewId,
                                    ProductId = review.ProductId,
                                    CustomerId = review.CustomerId,
                                    Rating = review.Rating,
                                    Comment = review.Comment,
                                    ImageUrl = review.ImageUrl,
                                    IsVerified = review.IsVerified,
                                    PurchaseDate = review.PurchaseDate,
                                    IsVisible = review.IsVisible,
                                    CreatedAt = review.CreatedAt,
                                    CustomerName = review.Customer?.FullName ?? "Người dùng ẩn danh"
                                };

                                if (!string.IsNullOrEmpty(review.CustomerId))
                                {
                                    try
                                    {
                                        reviewViewModel.CustomerName = review.Customer?.FullName ?? "Người dùng ẩn danh";

                                        var userResponse = await _apiClient.GetAsync($"/api/User/{review.CustomerId}");

                                        if (userResponse?.Status == 200 && userResponse.Data != null)
                                        {
                                            var userData = JsonConvert.DeserializeObject<UserDto>(userResponse.Data.ToString());

                                            if (userData != null)
                                            {
                                                if (!string.IsNullOrEmpty(userData.FullName))
                                                {
                                                    reviewViewModel.CustomerName = userData.FullName;
                                                }
                                                reviewViewModel.CustomerAvatar = userData.Avatar;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error getting User data: {ex.Message}");
                                    }
                                }

                                Reviews.Add(reviewViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        // Phương thức mặc định khi submit form không có handler
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("OnPostAsync called");

            // Không kiểm tra ModelState.IsValid ở đây để đảm bảo code luôn chạy
            try
            {
                var createReviewDto = new
                {
                    productId = ProductId,
                    customerId = "U001", // Fix cứng customerId
                    rating = NewReview.Rating,
                    comment = NewReview.Comment ?? "", // Đảm bảo comment không null
                    imageUrl = "string",
                    isVerified = false,
                    purchaseDate = DateTime.Now,
                    isVisible = true
                };

                Console.WriteLine($"Creating review: {JsonConvert.SerializeObject(createReviewDto)}");
                var response = await _apiClient.PostAsync("/reviews", createReviewDto);
                Console.WriteLine($"Create response: Status={response.Status}, Message={response.Message}");

                if (response.Status == 200 || response.Status == 201)
                {
                    NewReview = new CreateReviewModel();
                    TempData["SuccessMessage"] = "Đánh giá đã được tạo thành công!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi khi tạo đánh giá: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnPostAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            await LoadReviews();
            return Page();
        }

        // Phương thức xử lý chỉnh sửa đánh giá
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            Console.WriteLine("OnPostUpdateAsync called");
            Console.WriteLine($"EditReview: ReviewId={EditReview.ReviewId}, Rating={EditReview.Rating}, Comment={EditReview.Comment}");

            // Không kiểm tra ModelState.IsValid ở đây để đảm bảo code luôn chạy
            try
            {
                var updateReviewDto = new
                {
                    rating = EditReview.Rating,
                    comment = EditReview.Comment ?? "", // Đảm bảo comment không null
                    imageUrl = "string",
                    isVerified = false,
                    isVisible = true
                };

                Console.WriteLine($"Updating review: {JsonConvert.SerializeObject(updateReviewDto)}");
                var response = await _apiClient.PutAsync($"/reviews/{EditReview.ReviewId}", updateReviewDto);
                Console.WriteLine($"Update response: Status={response.Status}, Message={response.Message}");

                if (response.Status == 200 || response.Status == 204)
                {
                    TempData["SuccessMessage"] = "Đánh giá đã được cập nhật thành công!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi khi cập nhật đánh giá: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnPostUpdateAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            await LoadReviews();
            return Page();
        }

        // Phương thức xử lý xóa đánh giá
        public async Task<IActionResult> OnPostDeleteAsync(string reviewId)
        {
            Console.WriteLine($"OnPostDeleteAsync called with reviewId={reviewId}");

            try
            {
                Console.WriteLine($"Deleting review: {reviewId}");
                var response = await _apiClient.DeleteAsync($"/reviews/{reviewId}");
                Console.WriteLine($"Delete response: Status={response.Status}, Message={response.Message}");

                if (response.Status == 200 || response.Status == 204)
                {
                    TempData["SuccessMessage"] = "Đánh giá đã được xóa thành công!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Lỗi khi xóa đánh giá: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in OnPostDeleteAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
            }

            await LoadReviews();
            return Page();
        }

        private void CalculateRatingStatistics(List<ReviewDto> reviews)
        {
            if (reviews == null || !reviews.Any())
            {
                AverageRating = 0;
                return;
            }

            for (int i = 1; i <= 5; i++)
            {
                RatingCounts[i] = 0;
            }

            foreach (var review in reviews)
            {
                if (RatingCounts.ContainsKey(review.Rating))
                {
                    RatingCounts[review.Rating]++;
                }
            }

            double totalRating = reviews.Sum(r => r.Rating);
            AverageRating = Math.Round(totalRating / reviews.Count, 1);
        }
    }

    public class ReviewViewModel
    {
        public string ReviewId { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAvatar { get; set; }
    }

    public class CreateReviewModel
    {
        [Range(1, 5, ErrorMessage = "Vui lòng chọn từ 1 đến 5 sao")]
        public int Rating { get; set; } = 5;

        [Required(ErrorMessage = "Vui lòng nhập nội dung đánh giá")]
        public string Comment { get; set; }
    }

    public class EditReviewModel
    {
        public string ReviewId { get; set; }

        [Range(1, 5, ErrorMessage = "Vui lòng chọn từ 1 đến 5 sao")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung đánh giá")]
        public string Comment { get; set; }
    }

    public class ReviewDto
    {
        public string ReviewId { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ImageUrl { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedAt { get; set; }
        public CustomerDto Customer { get; set; }
        public object Product { get; set; }
    }

    public class CustomerDto
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string SkinTypeId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int LoyaltyPoints { get; set; }
        public string PreferredPaymentMethod { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalPoints { get; set; }
    }

    public class UserDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PaginatedResult<T>
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }

    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
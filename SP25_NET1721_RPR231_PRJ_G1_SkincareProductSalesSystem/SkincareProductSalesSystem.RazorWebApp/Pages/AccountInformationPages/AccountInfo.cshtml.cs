using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using Newtonsoft.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountInformationPages
{
    public class AccountInfoModel : PageModel
    {
        private readonly ApiClient _apiClient;

        public AccountInfoModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [BindProperty]
        public UserAccountViewModel UserAccount { get; set; } = new UserAccountViewModel();

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                // Gọi API User để lấy thông tin người dùng
                var response = await _apiClient.GetAsync($"/api/User/{userId}");

            if (response.Status == 200 && response.Data != null)
            {
                if (userData != null)
                {
                        // Ánh xạ dữ liệu vào ViewModel
                    UserAccount = new UserAccountViewModel
                    {
                        Email = userData.Email,
                        FullName = userData.FullName,
                    };

                    return Page();
                }
            }

            return RedirectToPage("/Index");
        }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                return Page();
            }

            {
                Email = UserAccount.Email,
                FullName = UserAccount.FullName,
            };


            if (response.Status == 200)
            {
                return RedirectToPage();
            }
            else
            {
                return Page();
            }
        }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
                return Page();
    }
        }
    }

    public class UserAccountViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }

        public IFormFile AvatarUpload { get; set; }
    }

    // DTO để deserialize dữ liệu từ API
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
}


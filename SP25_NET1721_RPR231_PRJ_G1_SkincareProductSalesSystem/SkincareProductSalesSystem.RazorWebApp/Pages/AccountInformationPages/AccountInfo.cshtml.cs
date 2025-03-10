using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            // Lấy userId từ cookie
            if (!Request.Cookies.TryGetValue("userId", out string userId) || string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Bạn cần đăng nhập để xem thông tin tài khoản.";
                return RedirectToPage("/Account/Login");
            }

            try
            {
                // Gọi API User để lấy thông tin người dùng
                var response = await _apiClient.GetAsync($"/api/User/{userId}");

                if (response.Status == 200 && response.Data != null)
                {
                    // Deserialize dữ liệu từ API
                    var userData = JsonConvert.DeserializeObject<UserDto>(response.Data.ToString());

                    if (userData != null)
                    {
                        // Ánh xạ dữ liệu vào ViewModel
                        UserAccount = new UserAccountViewModel
                        {
                            Username = userData.Username,
                            Email = userData.Email,
                            FullName = userData.FullName,
                            PhoneNumber = userData.PhoneNumber,
                            Avatar = userData.Avatar
                        };

                        return Page();
                    }
                }

                // Xử lý khi không tìm thấy dữ liệu người dùng
                ErrorMessage = "Không thể tải thông tin người dùng. Vui lòng thử lại sau.";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
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

            // Lấy userId từ cookie
            if (!Request.Cookies.TryGetValue("userId", out string userId) || string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Bạn cần đăng nhập để cập nhật thông tin tài khoản.";
                return RedirectToPage("/Account/Login");
            }

            try
            {
                // Tạo đối tượng request để cập nhật thông tin người dùng
                var updateRequest = new
                {
                    Username = UserAccount.Username,
                    Email = UserAccount.Email,
                    FullName = UserAccount.FullName,
                    PhoneNumber = UserAccount.PhoneNumber,
                    Avatar = UserAccount.Avatar
                };

                // Gọi API User để cập nhật thông tin
                var response = await _apiClient.PutAsync($"/api/User/{userId}", updateRequest);

                if (response.Status == 200)
                {
                    // Xử lý upload avatar nếu có
                    if (UserAccount.AvatarUpload != null && UserAccount.AvatarUpload.Length > 0)
                    {
                        // Xử lý upload avatar (có thể triển khai sau)
                        // Ví dụ: gọi API riêng để upload avatar
                    }

                    SuccessMessage = "Cập nhật thông tin thành công.";
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError("", $"Không thể cập nhật thông tin: {response.Message}");
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


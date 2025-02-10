using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

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

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _apiClient.GetAsync("/useraccounts/2");

            if (response.Status == 200 && response.Data != null)
            {
                var userData = JsonConvert.DeserializeObject<UserAccount>(response.Data.ToString());
                if (userData != null)
                {
                    UserAccount = new UserAccountViewModel
                    {
                        Username = userData.UserName,
                        Email = userData.Email,
                        FullName = userData.FullName,
                        PhoneNumber = userData.Phone,
                        //Avatar = userData.Avatar
                    };
                    return Page();
                }
            }

            // Handle when user data is not found or there's an error
            TempData["ErrorMessage"] = "Không thể tải thông tin người dùng. Vui lòng thử lại sau.";
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updateData = new
            {
                UserName = UserAccount.Username,
                Email = UserAccount.Email,
                FullName = UserAccount.FullName,
                Phone = UserAccount.PhoneNumber
            };

            var response = await _apiClient.PutAsync("/useraccounts/3", updateData);

            if (response.Status == 200)
            {
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError("", "Không thể cập nhật thông tin. Vui lòng thử lại.");
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
}


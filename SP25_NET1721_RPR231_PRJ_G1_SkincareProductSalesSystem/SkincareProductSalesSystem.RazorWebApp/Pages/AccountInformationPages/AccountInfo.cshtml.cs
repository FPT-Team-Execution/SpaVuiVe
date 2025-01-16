using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountInformationPages
{
    public class AccountInfoModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        [Phone]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Avatar { get; set; }

        [BindProperty]
        public IFormFile AvatarUpload { get; set; }

        public void OnGet()
        {
            // TODO: Populate the model with the user's current information
            // This is where you would typically fetch the user's data from your database
            Username = "johndoe";
            Email = "johndoe@example.com";
            FullName = "John Doe";
            PhoneNumber = "123-456-7890";
            Avatar = "/images/avatar.jpg"; // Example path to the user's avatar
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Update the user's information in your database
            // This is where you would typically save the changes to your database

            if (AvatarUpload != null)
            {
                // TODO: Handle file upload and update Avatar property
                // You would typically save the file to your server or cloud storage
                // and update the Avatar property with the new file path
            }

            // Redirect to prevent form resubmission
            return RedirectToPage();
        }
    }
}


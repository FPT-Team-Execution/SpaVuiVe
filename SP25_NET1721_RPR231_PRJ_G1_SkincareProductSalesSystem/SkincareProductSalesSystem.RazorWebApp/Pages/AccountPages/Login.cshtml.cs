using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using Newtonsoft.Json;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
    public class LoginModel : PageModel
    {

		private HttpClient _httpClient;
		private ApiClient _apiClient;

		[BindProperty]
		public LoginRequestModel LoginRequest { get; set; } = new LoginRequestModel();

		public string? ErrorMessage { get; set; }


		public LoginModel(IHttpClientFactory httpClientFactory, ApiClient apiClient)
		{
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.BaseAddress = new Uri("https://localhost:7000/api/");
			_apiClient = apiClient;
		}

		public void OnGet()
        {

        }

		[HttpPost]
		public async Task<IActionResult> OnPost()
		{
			try
			{
				if (!ModelState.IsValid)
				{
					ErrorMessage = "Invalid username or Password";
					return Page();
				}

				var response = await _apiClient.PostAsync("/login", LoginRequest);

				if (response.Status != 200)
				{
					ErrorMessage = response.Message;
					return Page();
				}
				var responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response.Data.ToString());
				var tokenHandler = new JwtSecurityTokenHandler();
				var accessToken = tokenHandler.ReadToken(responseModel.AccessToken) as JwtSecurityToken;
				if (accessToken == null)
				{
					return Page();
				}

				var userId = accessToken.Claims.FirstOrDefault(c => c.Type.Equals("UserId"))?.Value;
				var role = accessToken.Claims.FirstOrDefault(c => c.Type.Equals("role"))?.Value;

				var claims = new List<Claim>
						{
							new Claim(ClaimTypes.NameIdentifier, userId),
							new Claim(ClaimTypes.Role, role),
						};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

				Response.Cookies.Append("userId", userId);
				Response.Cookies.Append("Role", role);
				Response.Cookies.Append("AccessToken", responseModel.AccessToken);
				Response.Cookies.Append("RefreshToken", responseModel.RefreshToken);
				return RedirectToPage("/Index");

			} catch (Exception ex) 
			{
				Console.WriteLine(ex);
				ErrorMessage = ex.Message;
				return Page();
			};
		}
    }

	public class LoginRequestModel 
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, 1 uppercase character, 1 lowercase character, and 1 number")]
		public string Password { get; set; }
	}

	public class LoginResponseModel
	{
		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }
	}
}

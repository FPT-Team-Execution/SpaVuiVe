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

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
    public class LoginModel : PageModel
    {

		private HttpClient _httpClient;

		[BindProperty]
		public LoginRequestModel LoginRequest { get; set; }
		

		public LoginModel(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
			_httpClient.BaseAddress = new Uri("https://localhost:7000/api/");
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
					return Page();
				}

				var response = await _httpClient.PostAsJsonAsync("api/Auth/login", LoginRequest);
				if (!response.IsSuccessStatusCode)
				{
					return Page();
				}
				

				var responseModel = await JsonSerializer.DeserializeAsync<LoginResponseModel>(await response.Content.ReadAsStreamAsync());
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadToken(responseModel.AccessToken) as JwtSecurityToken;
				if (jwtToken == null)
				{
					return Page();
				}

				var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
				var roleId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

				var claims = new List<Claim>
						{
							new Claim(ClaimTypes.Name, userName),
							new Claim(ClaimTypes.Role, roleId),
						};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

				Response.Cookies.Append("UserName", userName);
				Response.Cookies.Append("Role", roleId);

				return RedirectToAction("Index", "Home");

			} catch (Exception ex) 
			{
				Console.WriteLine(ex);
				return Redirect("Error");
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

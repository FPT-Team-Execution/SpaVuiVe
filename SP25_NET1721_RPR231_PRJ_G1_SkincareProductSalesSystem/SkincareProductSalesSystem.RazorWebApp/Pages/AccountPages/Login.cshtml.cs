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
using SkincareProductSalesSystem.Common;
using Protos.AuthClient;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.AccountPages
{
    public class LoginModel : PageModel
    {
		private GrpcClient<AuthServiceGRPC.AuthServiceGRPCClient> _grpcClient;

		public LoginModel(GrpcClient<AuthServiceGRPC.AuthServiceGRPCClient> grpcClient)
		{
			_grpcClient = grpcClient;
		}

		[BindProperty]
		public LoginRequestModel LoginRequest { get; set; } = new LoginRequestModel();

		public string? ErrorMessage { get; set; }



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

				var response = await _grpcClient.Client.LoginAsync(new LoginRequestProto()
				{
					Username = LoginRequest.Username,
					Password = LoginRequest.Password
				});


				if (response.Status != 200)
				{
					ErrorMessage = response.Message;
					return Page();
				}
				
				var tokenHandler = new JwtSecurityTokenHandler();
				var accessToken = tokenHandler.ReadToken(response.Data.AccessToken) as JwtSecurityToken;
				if (accessToken == null)
				{
					return Page();
				}
				var uniqueName = accessToken.Claims.FirstOrDefault(c => c.Type.Equals("unique_name"))?.Value;
				var userId = accessToken.Claims.FirstOrDefault(c => c.Type.Equals("UserId"))?.Value;
				var role = accessToken.Claims.FirstOrDefault(c => c.Type.Equals("role"))?.Value;

				var claims = new List<Claim>
						{
							new Claim(ClaimTypes.NameIdentifier, userId),
							new Claim(ClaimTypes.Name, uniqueName),
							new Claim(ClaimTypes.Role, role),
						};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

				Response.Cookies.Append("userId", userId);
				Response.Cookies.Append("UniqueName", uniqueName);
				Response.Cookies.Append("Role", role);
				Response.Cookies.Append("AccessToken", response.Data.AccessToken);
				Response.Cookies.Append("RefreshToken", response.Data.RefreshToken);
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

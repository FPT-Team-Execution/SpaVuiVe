using Microsoft.AspNetCore.Http;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.Common.Utils;

namespace SkincareProductSalesSystem.RazorWebApp.Models.Base
{
    public class ApiClient
    {
        private const string baseDomain = "https://localhost:7000";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetAccessTokenFromCookie()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[Const.ACCESS_TOKEN_COOKIE];
        }

        public async Task<ServiceResult> GetAsync(string endpoint, string? accessToken = null)
        {
            try
            {
                string url = baseDomain + endpoint;
                // Use provided token or get from cookie
                accessToken ??= GetAccessTokenFromCookie();

                Dictionary<string, string>? headers = null;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    headers = new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {accessToken}" }
                };
                }

                var response = await WebUtil.GetAsync(url, headers, accessToken);
                var result = WebUtil.HandleResponse<ServiceResult>(response);
                return result;
            }
            catch (Exception)
            {

                return new ServiceResult(500, "Lỗi");
            }
        }

        public async Task<ServiceResult> PostAsync(string endpoint, object? body, string? accessToken = null)
        {
            try
            {
                string url = baseDomain + endpoint;
                // Use provided token or get from cookie
                accessToken ??= GetAccessTokenFromCookie();

                Dictionary<string, string>? headers = null;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    headers = new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {accessToken}" }
                };
                }

                var response = await WebUtil.PostAsync(url, body, headers, accessToken);
                var result = WebUtil.HandleResponse<ServiceResult>(response);
                return result;
            }
            catch (Exception)
            {
                return new ServiceResult(500, "Lỗi");
            }
        }

        public async Task<ServiceResult> PutAsync(string endpoint, object? body, string? accessToken = null)
        {
            try
            {
                string url = baseDomain + endpoint;
                // Use provided token or get from cookie
                accessToken ??= GetAccessTokenFromCookie();

                Dictionary<string, string>? headers = null;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    headers = new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {accessToken}" }
                };
                }

                var response = await WebUtil.PutAsync(url, body, headers, accessToken);
                var result = WebUtil.HandleResponse<ServiceResult>(response);
                return result;
            }
            catch (Exception)
            {
                return new ServiceResult(500, "Lỗi");

            }
        }

        public async Task<ServiceResult> DeleteAsync(string endpoint, string? accessToken = null)
        {
            try
            {
                string url = baseDomain + endpoint;
                // Use provided token or get from cookie
                accessToken ??= GetAccessTokenFromCookie();

                Dictionary<string, string>? headers = null;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    headers = new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {accessToken}" }
                };
                }

                var response = await WebUtil.DeleteAsync(url, headers, accessToken);
                var result = WebUtil.HandleResponse<ServiceResult>(response);
                return result;
            }
            catch (Exception)
            {
                return new ServiceResult(500, "Lỗi");

            }
        }
    }
}
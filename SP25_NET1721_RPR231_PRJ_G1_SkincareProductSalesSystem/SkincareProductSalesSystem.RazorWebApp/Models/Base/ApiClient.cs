using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.Common.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            catch (Exception ex)
            {

                return new ServiceResult(500, ex.Message);
            }
        }

        public async Task<ServiceResult> GetMinhAsync<T>(string endpoint, string? accessToken = null)
        {
            try
            {
                string url = baseDomain + endpoint;
                Console.WriteLine($"Full URL: {url}"); // Debug line

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
                Console.WriteLine($"Response received: {response != null}"); // Debug line

                // Add this before handling the response
                var rawContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw response: {rawContent}"); // Debug line

                var serviceResultJson = response.Content.ReadAsStringAsync().Result;
                var serviceResultJsonDeserializeObject = (serviceResultJson != null)? new ServiceResult
                {
                    Data = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result),
                    Message = "Success",
                    Status = 200
                } : new ServiceResult
                {
                    Message = "Fail",
                    Status = 500
                };

                return serviceResultJsonDeserializeObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}"); // Debug line
                return new ServiceResult(500, ex.Message);
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
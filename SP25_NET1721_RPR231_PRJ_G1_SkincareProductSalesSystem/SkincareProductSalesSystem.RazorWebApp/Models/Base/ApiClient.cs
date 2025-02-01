using SkincareProductSalesSystem.Common.Utils;
using System.Security.Policy;

namespace SkincareProductSalesSystem.RazorWebApp.Models.Base
{
    public class ApiClient
    {
        private const string baseDomain = "https://localhost:7000";
        public async Task<ServiceResult> GetAsync(string endpoint, string accessToken)
        {
            string url = baseDomain + endpoint;
            Dictionary<string, string>? headers = null;
            if (string.IsNullOrEmpty(accessToken))
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
        public async Task<ServiceResult> PostAsync(string endpoint, object? body, string accessToken)
        {
            string url = baseDomain + endpoint;
            Dictionary<string, string>? headers = null;
            if (string.IsNullOrEmpty(accessToken))
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
        public async Task<ServiceResult> PutAsync(string endpoint, object? body, string accessToken)
        {
            string url = baseDomain + endpoint;
            Dictionary<string, string>? headers = null;
            if (string.IsNullOrEmpty(accessToken))
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
        public async Task<ServiceResult> DeleteAsync(string endpoint, string accessToken)
        {
            string url = baseDomain + endpoint;
            Dictionary<string, string>? headers = null;
            if (string.IsNullOrEmpty(accessToken))
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
    }
}

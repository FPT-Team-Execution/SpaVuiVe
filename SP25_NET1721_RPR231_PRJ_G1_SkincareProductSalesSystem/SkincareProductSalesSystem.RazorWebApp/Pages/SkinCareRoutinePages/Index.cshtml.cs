using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using SkincareProductSalesSystem.Repositories.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinCareRoutinePages
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class PaginatedData<T>
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;
        public List<SkinCareRoutine>? SkinCareRoutines { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public IndexModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync(int page = 1, int size = 10)
        {
            try
            {
                CurrentPage = page;
                PageSize = size;

                var response = await _apiClient.GetAsync($"/skin-care-routines?page={page}&size={size}");

                if (response.Status == 200 && response.Data != null)
                {
                   
                    var dataObj = JObject.Parse(response.Data.ToString());

                    
                    if (dataObj["items"] != null)
                    {
                        SkinCareRoutines = dataObj["items"].ToObject<List<SkinCareRoutine>>();
                        TotalPages = dataObj["totalPages"]?.Value<int>() ?? 1;
                        CurrentPage = dataObj["page"]?.Value<int>() ?? 1;
                        PageSize = dataObj["size"]?.Value<int>() ?? 10;
                    }
                    else
                    {
                        SkinCareRoutines = new List<SkinCareRoutine>();
                        TotalPages = 1;
                    }
                }
                else
                {
                    SkinCareRoutines = new List<SkinCareRoutine>();
                    TotalPages = 1;
                }
            }
            catch (Exception ex)
            {
                // Log error here if needed
                SkinCareRoutines = new List<SkinCareRoutine>();
                TotalPages = 1;
            }
        }
    }
}
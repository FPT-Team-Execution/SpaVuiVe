using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Protos.PromotionClient;
using SkincareProductSalesSystem.Common;
using SkincareProductSalesSystem.RazorWebApp.Models.Base;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class DeleteModel : PageModel
    {


		private ApiClient _apiClient;

		public DeleteModel(ApiClient apiClient)
		{
			_apiClient = apiClient;
		}

		[BindProperty]
        public Promotion Promotion { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
			if (id == null)
			{
				return RedirectToPage("./Index");
			}

			var response = await _apiClient.GetAsync($"/api/Promotion/{id}");

			if (response.Status != 200)
			{
				return RedirectToPage("./Index");
			}

			Promotion = JsonConvert.DeserializeObject<Promotion>(response.Data.ToString());
			return Page();
		}

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
				return RedirectToPage("./Index");
			}

			var response = await _apiClient.DeleteAsync($"/api/Promotion/{id}");

			return RedirectToPage("./Index");
		}
    }
}

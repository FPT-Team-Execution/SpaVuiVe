using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionPages
{
    public class DetailsModel : PageModel
    {
        private readonly SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext _context;

        public DetailsModel(SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext context)
        {
            _context = context;
        }

        public Promotion Promotion { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FirstOrDefaultAsync(m => m.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }
            else
            {
                Promotion = promotion;
            }
            return Page();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionUsagePages
{
    public class DeleteModel : PageModel
    {
        private readonly SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext _context;

        public DeleteModel(SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PromotionUsage PromotionUsage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionusage = await _context.PromotionUsages.FirstOrDefaultAsync(m => m.UsageId == id);

            if (promotionusage == null)
            {
                return NotFound();
            }
            else
            {
                PromotionUsage = promotionusage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotionusage = await _context.PromotionUsages.FindAsync(id);
            if (promotionusage != null)
            {
                PromotionUsage = promotionusage;
                _context.PromotionUsages.Remove(PromotionUsage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

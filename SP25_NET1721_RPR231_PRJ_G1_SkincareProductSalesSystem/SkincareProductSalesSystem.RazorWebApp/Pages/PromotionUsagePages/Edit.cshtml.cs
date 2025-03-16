using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Models;

namespace SkincareProductSalesSystem.RazorWebApp.Pages.PromotionUsagePages
{
    public class EditModel : PageModel
    {
        private readonly SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext _context;

        public EditModel(SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext context)
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

            var promotionusage =  await _context.PromotionUsages.FirstOrDefaultAsync(m => m.UsageId == id);
            if (promotionusage == null)
            {
                return NotFound();
            }
            PromotionUsage = promotionusage;
           ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
           ViewData["PromotionId"] = new SelectList(_context.Promotions, "PromotionId", "PromotionId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PromotionUsage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionUsageExists(PromotionUsage.UsageId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PromotionUsageExists(string id)
        {
            return _context.PromotionUsages.Any(e => e.UsageId == id);
        }
    }
}

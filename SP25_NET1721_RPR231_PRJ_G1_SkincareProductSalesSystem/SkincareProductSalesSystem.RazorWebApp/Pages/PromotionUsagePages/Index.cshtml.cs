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
    public class IndexModel : PageModel
    {
        private readonly SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext _context;

        public IndexModel(SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext context)
        {
            _context = context;
        }

        public IList<PromotionUsage> PromotionUsage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PromotionUsage = await _context.PromotionUsages
                .Include(p => p.Order)
                .Include(p => p.Promotion).ToListAsync();
        }
    }
}

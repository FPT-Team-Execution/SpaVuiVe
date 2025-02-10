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

namespace SkincareProductSalesSystem.RazorWebApp.Pages.SkinTypePages
{
    public class EditModel : PageModel
    {
        private readonly SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext _context;

        public EditModel(SkincareProductSalesSystem.Repositories.Database.SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SkinType SkinType { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skintype =  await _context.SkinTypes.FirstOrDefaultAsync(m => m.SkinTypeId == id);
            if (skintype == null)
            {
                return NotFound();
            }
            SkinType = skintype;
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

            _context.Attach(SkinType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkinTypeExists(SkinType.SkinTypeId))
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

        private bool SkinTypeExists(string id)
        {
            return _context.SkinTypes.Any(e => e.SkinTypeId == id);
        }
    }
}

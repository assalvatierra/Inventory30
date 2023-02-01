using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec.Category
{
    public class AddModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public AddModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Description");
            ViewData["InvItemSysDefinedSpecsId"] = new SelectList(_context.InvItemSysDefinedSpecs, "Id", "SpecName", id);
            ViewData["InvCategoryList"] = _context.InvCategories.ToList();
            return Page();
        }

        [BindProperty]
        public InvCategorySpecDef InvCategorySpecDefs { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvCategorySpecDefs.Add(InvCategorySpecDefs);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}

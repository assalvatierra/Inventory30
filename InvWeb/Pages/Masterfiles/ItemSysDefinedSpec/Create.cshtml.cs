using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public InvItemSysDefinedSpecs InvItemSysDefinedSpecs { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItemSysDefinedSpecs.Add(InvItemSysDefinedSpecs);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

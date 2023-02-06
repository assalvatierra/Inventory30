using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec.Category
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvCategorySpecDef InvCategorySpecDefs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCategorySpecDefs = await _context.InvCategorySpecDefs
                .Include(i => i.InvCategory)
                .Include(i => i.InvItemSysDefinedSpec)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvCategorySpecDefs == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCategorySpecDefs = await _context.InvCategorySpecDefs.FindAsync(id);

            if (InvCategorySpecDefs != null)
            {
                _context.InvCategorySpecDefs.Remove(InvCategorySpecDefs);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}

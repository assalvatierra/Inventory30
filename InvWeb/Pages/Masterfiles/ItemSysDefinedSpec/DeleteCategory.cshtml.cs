using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec
{
    public class DeleteCategoryModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteCategoryModel(InvWeb.Data.ApplicationDbContext context)
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

            return RedirectToPage("./Index");
        }
    }
}
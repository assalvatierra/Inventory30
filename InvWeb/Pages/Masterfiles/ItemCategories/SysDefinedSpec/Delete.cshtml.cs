using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemCategories.SysDefinedSpec
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvCategorySpecDef InvCategorySpecDef { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvCategorySpecDef = await _context.InvCategorySpecDefs
                .Include(i=>i.InvCategory)
                .Include(i => i.InvItemSysDefinedSpec)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvCategorySpecDef == null)
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

            InvCategorySpecDef = await _context.InvCategorySpecDefs.FindAsync(id);

            if (InvCategorySpecDef != null)
            {
                _context.InvCategorySpecDefs.Remove(InvCategorySpecDef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}

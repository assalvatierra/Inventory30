using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItemSysDefinedSpecs InvItemSysDefinedSpecs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemSysDefinedSpecs = await _context.InvItemSysDefinedSpecs
                .Include(i => i.InvCategorySpecDefs)
                    .ThenInclude(i => i.InvCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemSysDefinedSpecs == null)
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

            InvItemSysDefinedSpecs = await _context.InvItemSysDefinedSpecs.FindAsync(id);

            if (InvItemSysDefinedSpecs != null)
            {
                _context.InvItemSysDefinedSpecs.Remove(InvItemSysDefinedSpecs);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

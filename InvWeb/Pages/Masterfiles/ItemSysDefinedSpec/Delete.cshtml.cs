using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
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

            InvItemSysDefinedSpecs = await _context.InvItemSysDefinedSpecs.FirstOrDefaultAsync(m => m.Id == id);

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

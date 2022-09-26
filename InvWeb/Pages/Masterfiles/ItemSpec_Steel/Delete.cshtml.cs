using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpec_Steel
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItemSpec_Steel = await _context.InvItemSpec_Steel.FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemSpec_Steel == null)
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

            InvItemSpec_Steel = await _context.InvItemSpec_Steel.FindAsync(id);

            if (InvItemSpec_Steel != null)
            {
                _context.InvItemSpec_Steel.Remove(InvItemSpec_Steel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

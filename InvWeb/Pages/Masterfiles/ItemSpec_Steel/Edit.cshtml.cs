using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpec_Steel
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
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

            InvItemSpec_Steel = await _context.InvItemSpec_Steel
                .Include(i => i.InvItem)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvItemSpec_Steel == null)
            {
                return NotFound();
            }
            ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(InvItemSpec_Steel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvPoHdrExists(InvItemSpec_Steel.Id))
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

        private bool InvPoHdrExists(int id)
        {
            return _context.InvPoHdrs.Any(e => e.Id == id);
        }
    }
}

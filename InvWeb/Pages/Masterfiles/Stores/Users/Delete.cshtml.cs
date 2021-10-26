using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.Stores.Users
{
    public class DeleteModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DeleteModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InvStoreUser InvStoreUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvStoreUser = await _context.InvStoreUsers
                .Include(i => i.InvStore).FirstOrDefaultAsync(m => m.Id == id);

            if (InvStoreUser == null)
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

            InvStoreUser = await _context.InvStoreUsers.FindAsync(id);

            if (InvStoreUser != null)
            {
                _context.InvStoreUsers.Remove(InvStoreUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { id = InvStoreUser.InvStoreId });
        }
    }
}

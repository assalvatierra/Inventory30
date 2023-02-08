using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using System.Security.Claims;
using InvWeb.Data.Services;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.Stores.Users
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        //TODO: add interface IUserServices
        private readonly UserServices userServices;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
            //userServices = new UserServices(context);
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

            ViewData["InvStoreId"] = new SelectList(_context.InvStores, "Id", "StoreName");
            ViewData["UserId"] = new SelectList(userServices.GetUserList(), "Username", "Username");
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

            _context.Attach(InvStoreUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvStoreUserExists(InvStoreUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { id = InvStoreUser.InvStoreId });
        }

        private bool InvStoreUserExists(int id)
        {
            return _context.InvStoreUsers.Any(e => e.Id == id);
        }
    }
}

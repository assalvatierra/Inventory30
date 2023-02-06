using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using System.Security.Claims;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace InvWeb.Pages.Masterfiles.Stores.Users
{
    public class CreateModel : PageModel
    {
        private readonly IdentityDbContext _securitycontext;
        private readonly ApplicationDbContext _context;
        private readonly UserServices userServices;

        public CreateModel(IdentityDbContext securitycontext, ApplicationDbContext context)
        {
            _securitycontext = securitycontext;
            _context = context;
            userServices = new UserServices(_securitycontext);
        }

        public IActionResult OnGet(int id)
        {
            //_ = _context.Users;
            ViewData["InvStoreId"] = new SelectList(_context.InvStores, "Id", "StoreName", id);
            ViewData["UserId"] = new SelectList(userServices.GetUserList(), "Username", "Username");
            ViewData["storeId"] = id;
            return Page();
        }

        [BindProperty]
        public InvStoreUser InvStoreUser { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvStoreUsers.Add(InvStoreUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = InvStoreUser.InvStoreId });
        }
    }
}

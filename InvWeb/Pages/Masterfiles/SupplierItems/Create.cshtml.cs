using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;
using System.Security.Claims;

namespace InvWeb.Pages.Masterfiles.SupplierItems
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                RedirectToAction("../Supplier/Index");
            }

            ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description");
            ViewData["InvSupplierId"] = new SelectList(_context.InvSuppliers, "Id", "Name", id);

            ViewData["UserId"] = this.User.FindFirstValue(ClaimTypes.Name);
            ViewData["SupplierId"] = id;
            return Page();
        }

        [BindProperty]
        public InvSupplierItem InvSupplierItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvSupplierItems.Add(InvSupplierItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = InvSupplierItem.InvSupplierId });
        }
    }
}

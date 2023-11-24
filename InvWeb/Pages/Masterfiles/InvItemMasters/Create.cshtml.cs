using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.DTO.Common.TrxDetails;
using Modules.Inventory;

namespace InvWeb.Pages.Masterfiles.InvItemMasters
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            ViewData["InvItems"] = new SelectList(_context.Set<InvItem>(), "Id", "Description");
            ViewData["InvItemBrands"] = new SelectList(_context.Set<InvItemBrand>(), "Id", "Name");
            ViewData["InvItemOrigins"] = new SelectList(_context.Set<InvItemOrigin>(), "Id", "Name");
            ViewData["InvUoms"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");

            return Page();
        }

        [BindProperty]
        public InvItemMaster InvItemMaster { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItemMasters.Add(InvItemMaster);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

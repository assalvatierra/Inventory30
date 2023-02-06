using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion
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
            ViewData["InvUomId_base"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["InvUomId_into"] = new SelectList(_context.InvUoms, "Id", "uom");
            return Page();
        }

        [BindProperty]
        public InvUomConversion InvUomConversion { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvUomConversions.Add(InvUomConversion);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

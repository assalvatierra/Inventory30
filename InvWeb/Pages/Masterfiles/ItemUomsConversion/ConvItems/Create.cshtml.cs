using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion.ConvItems
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["InvClassificationId"] = new SelectList(_context.InvClassifications, "Id", "Id");
        ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id");
        ViewData["InvUomConversionId"] = new SelectList(_context.InvUomConversions, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public InvUomConvItem InvUomConvItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvUomConvItem.Add(InvUomConvItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

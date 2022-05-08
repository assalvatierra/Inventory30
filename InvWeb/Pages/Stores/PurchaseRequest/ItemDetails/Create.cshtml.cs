using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using WebDBSchema.Models;
using InvWeb.Data.Services;

namespace InvWeb.Pages.Stores.PurchaseRequest.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
        }

        public IActionResult OnGet(int? hdrId)
        {
            if (hdrId == null)
            {
                return NotFound();
            }

            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList();
            ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description");
            ViewData["InvPoHdrId"] = new SelectList(_context.InvPoHdrs, "Id", "Id", hdrId);
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["HdrId"] = hdrId;
            return Page();
        }

        [BindProperty]
        public InvPoItem InvPoItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvPoItems.Add(InvPoItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvPoItem.InvPoHdrId });
        }
    }
}

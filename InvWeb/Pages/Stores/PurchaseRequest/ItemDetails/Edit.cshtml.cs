using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using InvWeb.Data.Interfaces;

namespace InvWeb.Pages.Stores.PurchaseRequest.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;
        private readonly UomServices _uomServices;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
        }

        [BindProperty]
        public InvPoItem InvPoItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoItem = await _context.InvPoItems
                .Include(i => i.InvItem)
                .Include(i => i.InvPoHdr)
                .Include(i => i.InvUom).FirstOrDefaultAsync(m => m.Id == id);

            if (InvPoItem == null)
            {
                return NotFound();
            }
            
            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList(InvPoItem.InvItemId);
            ViewData["InvPoHdrId"] = new SelectList(_context.InvPoHdrs, "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(InvTrxDtl.InvItemId), "Id", "uom");
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

            _context.Attach(InvPoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvPoItemExists(InvPoItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Details", new { id = InvPoItem.InvPoHdrId });
        }

        private bool InvPoItemExists(int id)
        {
            return _context.InvPoItems.Any(e => e.Id == id);
        }
    }
}

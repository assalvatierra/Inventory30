using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;
using InvWeb.Data.Services;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;

        public EditModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxDtl = await _context.InvTrxDtls
                .Include(i => i.InvItem)
                .Include(i => i.InvTrxHdr)
                .Include(i => i.InvUom).FirstOrDefaultAsync(m => m.Id == id);

            if (InvTrxDtl == null)
            {
                return NotFound();
            }

            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList(InvTrxDtl.InvItemId);
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description");
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

            _context.Attach(InvTrxDtl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvTrxDtlExists(InvTrxDtl.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
        }

        private bool InvTrxDtlExists(int id)
        {
            return _context.InvTrxDtls.Any(e => e.Id == id);
        }
    }
}

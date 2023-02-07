using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemServices _itemServices;
        private readonly UomServices _uomServices;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
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


            ViewData["InvItemId"] = new SelectList(_itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                    .Select(x => new
                                    {
                                        Name = String.Format("{0} - {1} - {2} {3}",
                                        x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                        Value = x.Id
                                    }), "Value", "Name", InvTrxDtl.InvItemId);
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(InvTrxDtl.InvItemId), "Id", "uom", InvTrxDtl.InvItem.InvUomId);
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

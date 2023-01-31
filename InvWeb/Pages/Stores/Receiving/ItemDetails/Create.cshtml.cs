using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using InvWeb.Data.Services;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;
        private readonly UomServices _uomServices;

        public CreateModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
        }

        public IActionResult OnGet(int? hdrId)
        {
            if (hdrId == null) 
            {
                hdrId ??= 0;
            }

            InvTrxDtl = new InvTrxDtl();
            InvTrxDtl.InvItemId = 2;
            InvTrxDtl.InvTrxHdrId =(int)hdrId;

            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList();
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(InvTrxDtl.InvItemId), "Id", "uom");
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id", hdrId);
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description", 1);
            ViewData["HdrId"] = hdrId;
            return Page();
        }

        [BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvTrxDtls.Add(InvTrxDtl);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = InvTrxDtl.InvTrxHdrId });
        }

    }
}

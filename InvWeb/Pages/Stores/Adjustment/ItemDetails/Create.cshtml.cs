using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Adjustment.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemServices _itemServices;
        private readonly IUomServices _uomServices;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
        }

        public IActionResult OnGet(int hdrId)
        {
            ViewData["InvItemId"] = new SelectList(_itemServices.GetInvItemsSelectList().Include(i => i.InvCategory)
                                    .Select(x => new
                                    {
                                        Name = String.Format("{0} - {1} - {2} {3}",
                                        x.Code, x.InvCategory.Description, x.Description, x.Remarks),
                                        Value = x.Id
                                    }), "Value", "Name");
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id", hdrId);
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(InvTrxDtl.InvItemId), "Id", "uom");
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description");
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

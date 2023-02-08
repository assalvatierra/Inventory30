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
using Modules.Inventory;

namespace InvWeb.Pages.Stores.PurchaseRequest.ItemDetails
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

        public IActionResult OnGet(int? hdrId)
        {
            if (hdrId == null)
            {
                return NotFound();
            }

            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList();
            //ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Description");
            ViewData["InvPoHdrId"] = new SelectList(_context.InvPoHdrs, "Id", "Id", hdrId);
            ViewData["InvUomId"] = new SelectList(_uomServices.GetUomSelectListByItemId(InvPoItem.InvItemId), "Id", "uom");
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

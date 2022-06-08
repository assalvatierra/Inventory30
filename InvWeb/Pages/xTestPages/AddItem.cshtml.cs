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

namespace InvWeb.Pages.xTestPages
{
    public class AddItemModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ItemServices _itemServices;


        public AddItemModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            _itemServices = new ItemServices(context);
        }

        public IActionResult OnGet()
        {
            ViewData["InvItemId"] = _itemServices.GetInvItemsSelectList();
            //ViewData["InvItemId"] = new SelectList(_context.InvItems, "Id", "Id");
            ViewData["InvTrxDtlOperatorId"] = new SelectList(_context.InvTrxDtlOperators, "Id", "Description");
            ViewData["InvTrxHdrId"] = new SelectList(_context.InvTrxHdrs, "Id", "Id");
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
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

            return RedirectToPage("./Index");
        }
    }
}

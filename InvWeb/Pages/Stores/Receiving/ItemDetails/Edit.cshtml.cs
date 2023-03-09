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
using Modules.Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Receiving;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;

        public EditModel(ApplicationDbContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(_context, _logger);
        }

        [BindProperty]
        public ReceivingItemDtlsCreateEditModel ItemDtlsEditModel { get; set; }
        //[BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            InvTrxDtl = await _itemDtlsServices.GetInvDtlsByIdAsync((int)id);

            if (InvTrxDtl == null)
            {
                return NotFound();
            }

            //refactored
            ItemDtlsEditModel = _itemDtlsServices.GeReceivingItemDtlsEditModel_OnEditOnGet(InvTrxDtl);

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

            // _context.Attach(InvTrxDtl).State = EntityState.Modified;
            _itemDtlsServices.EditInvDtls(ItemDtlsEditModel.InvTrxDtl);
            try
            {
                //await  _context.SaveChangesAsync();
                await _itemDtlsServices.SaveChangesAsync();
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

            return RedirectToPage("../Details", new { id = ItemDtlsEditModel.InvTrxDtl.InvTrxHdrId });
        }

        private bool InvTrxDtlExists(int id)
        {
            return _itemDtlsServices.InvTrxDtlsExists(id);
        }
    }
}

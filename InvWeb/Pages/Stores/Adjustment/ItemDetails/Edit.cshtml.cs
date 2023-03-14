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
using CoreLib.DTO.Common.TrxDetails;

namespace InvWeb.Pages.Stores.Adjustment.ItemDetails
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemServices _itemServices;
        private readonly IUomServices _uomServices;

        private readonly ILogger<CreateModel> _logger;
        private readonly IItemDtlsServices itemDtlsServices;

        public EditModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            itemDtlsServices = new ItemDtlsServices(_context, _logger);

            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
        }

        [BindProperty]
        public TrxItemsCreateEditModel ItemsEditModel { get; set; }
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvTrxDtl = await itemDtlsServices.GetInvDtlsByIdAsync((int)id);

            if (InvTrxDtl == null)
            {
                return NotFound();
            }

            ItemsEditModel = itemDtlsServices.GeItemDtlsEditModel_OnEditOnGet(InvTrxDtl);

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

            itemDtlsServices.EditInvDtls(ItemsEditModel.InvTrxDtl);

            try
            {
                await itemDtlsServices.SaveChangesAsync();
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

            return RedirectToPage("../Details", new { id = ItemsEditModel.InvTrxDtl.InvTrxHdrId });
        }

        private bool InvTrxDtlExists(int id)
        {
            return _context.InvTrxDtls.Any(e => e.Id == id);
        }
    }
}

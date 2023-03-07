using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data.Services;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;
using CoreLib.DTO.Receiving;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Releasing;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(_context, _logger);
        }

        [BindProperty]
        public ReceivingItemDtlsCreateEditModel ItemDtlsCreateModel { get; set; }
        public InvTrxDtl InvTrxDtl { get; set; }

        public IActionResult OnGet(int? hdrId)
        {
            if (hdrId == null) 
            {
                hdrId ??= 0;
            }


            ItemDtlsCreateModel = _itemDtlsServices.GeReceivingItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl, (int)hdrId);


            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _itemDtlsServices.CreateInvDtls(ItemDtlsCreateModel.InvTrxDtl);
            await _itemDtlsServices.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = ItemDtlsCreateModel.InvTrxDtl.InvTrxHdrId });
        }

    }
}

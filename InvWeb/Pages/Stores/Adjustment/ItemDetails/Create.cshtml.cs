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
using Microsoft.EntityFrameworkCore;
using Modules.Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Common.TrxDetails;

namespace InvWeb.Pages.Stores.Adjustment.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemDtlsServices itemDtlsServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            itemDtlsServices = new ItemDtlsServices(_context, _logger);
        }

        [BindProperty]
        public TrxItemsCreateEditModel ItemsCreateModel { get; set; }
        public InvTrxDtl InvTrxDtl { get; set; }

        public IActionResult OnGet(int hdrId)
        {
            ItemsCreateModel = itemDtlsServices.GeItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl, hdrId);

            return Page();
        }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.InvTrxDtls.Add(ItemsCreateModel.InvTrxDtl);
            //await _context.SaveChangesAsync();

            itemDtlsServices.CreateInvDtls(ItemsCreateModel.InvTrxDtl);
            await itemDtlsServices.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = ItemsCreateModel.InvTrxDtl.InvTrxHdrId });
        }
    }
}

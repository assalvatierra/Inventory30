using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Common.TrxDetails;
using Modules.Inventory;

namespace InvWeb.Pages.Stores.Adjustment.ItemDetails
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;

        public DeleteModel(ApplicationDbContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(_context, _logger);
        }


        [BindProperty]
        public TrxDetailsItemDeleteModel ItemDetailsDeleteModel { get; set; }
        public InvTrxDtl InvTrxDtl { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ItemDetailsDeleteModel = await _itemDtlsServices.GetTrxDetailsModel_OnDeleteAsync((int)id);

            if (ItemDetailsDeleteModel.InvTrxDtl == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ItemDetailsDeleteModel.InvTrxDtl = await _itemDtlsServices.GetInvDtlsByIdAsync((int)id);

            if (ItemDetailsDeleteModel.InvTrxDtl != null)
            {
                _itemDtlsServices.DeleteInvDtls(ItemDetailsDeleteModel.InvTrxDtl);
                await _itemDtlsServices.SaveChangesAsync();

            }

            return RedirectToPage("../Details", new { id = ItemDetailsDeleteModel.InvTrxDtl.InvTrxHdrId });
        }
    }
}

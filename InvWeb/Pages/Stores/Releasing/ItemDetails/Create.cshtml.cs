using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data.Services;
using CoreLib.Inventory.Models;
using CoreLib.Inventory.Models.Items;
using Microsoft.Extensions.Logging;
using CoreLib.Inventory.Interfaces;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory;
using CoreLib.DTO.Releasing;

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IItemServices _itemServices;
        private readonly IStoreServices _storeServices;
        private readonly IUomServices _uomServices;
        private readonly IItemDtlsServices _itemDtlsServices;
        private readonly IItemTrxServices _itemTrxServices;



        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _itemServices = new ItemServices(context);
            _uomServices = new UomServices(context);
            _storeServices = new StoreServices(context, _logger);
            _itemDtlsServices = new ItemDtlsServices(context, _logger);
            _itemTrxServices = new ItemTrxServices(context, _logger);
        }

        [BindProperty]
        public ReleasingItemDtlsCreateEditModel ItemDtlsCreateEditModel { get; set; }

        //[BindProperty]
        public InvTrxDtl InvTrxDtl { get; set; }

        public IActionResult OnGet(int? hdrId, int? invItemId)
        {
            if (hdrId == null)
            {
                return NotFound();
            }

            if (invItemId == null)
            {
                invItemId = 2;
            }

            int itemId = GetDefaultInvitemId(invItemId);

            ItemDtlsCreateEditModel = _itemDtlsServices.GetReleasingItemTrxDtlsModel_OnCreateOnGet(InvTrxDtl, (int)hdrId, (int)itemId);

            ItemDtlsCreateEditModel.InvTrxDtl.InvItemId = itemId;

            ViewData["SelectedItemId"] = invItemId;
            ViewData["HdrId"] = hdrId;

            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (ItemDtlsCreateEditModel.InvTrxDtl.LotNo == 0 || ItemDtlsCreateEditModel.InvTrxDtl.LotNo == null)
                {
                    //requires LotNo
                    return RedirectToPage("../Details", new { id = ItemDtlsCreateEditModel.InvTrxDtl.InvTrxHdrId });
                }

                _itemDtlsServices.CreateInvDtls(ItemDtlsCreateEditModel.InvTrxDtl);
                await _itemDtlsServices.SaveChangesAsync();

                return RedirectToPage("../Details", new { id = ItemDtlsCreateEditModel.InvTrxDtl.InvTrxHdrId });

            }
            catch (Exception ex)
            {

                _logger.LogError("Stores/Releasing/Create: Unable to OnPostAsync " + ex.Message);
                return RedirectToPage("../Details", new { id = ItemDtlsCreateEditModel.InvTrxDtl.InvTrxHdrId });
            }
        }

        private int GetDefaultInvitemId(int? invItemId)
        {
            return invItemId == null ? 1 : (int)invItemId;
        }
    }
}

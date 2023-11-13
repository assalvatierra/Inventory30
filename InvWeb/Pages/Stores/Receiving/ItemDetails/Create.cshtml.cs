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
using InvWeb.Pages.Shared.Components.Dialog;
using CoreLib.DTO.Common.Dialog;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemDtlsServices _itemDtlsServices;
        private readonly IItemServices _itemServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemDtlsServices = new ItemDtlsServices(_context, _logger);
            _itemServices = new ItemServices(context);
        }

        [BindProperty]
        public ReceivingItemDtlsCreateEditModel ItemDtlsCreateModel { get; set; }
        public InvTrxDtl InvTrxDtl { get; set; }

        public IActionResult OnGet(int? hdrId, int? itemId)
        {
            if (hdrId == null) 
            {
                hdrId ??= 0;
            }


            if (itemId == null)
            {
                itemId ??= 0;
            }


            ItemDtlsCreateModel = _itemDtlsServices.GeReceivingItemDtlsCreateModel_OnCreateOnGet(InvTrxDtl, (int)hdrId, (int)itemId);

            ViewData["SelectedItemId"] = itemId;
            ViewData["DialogItems"] = ConvertItemsToDialogItems(_itemServices.GetInvItemsSelectList().ToList());

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
        public IActionResult SearchItem()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("../SearchItem", new { id = ItemDtlsCreateModel.InvTrxDtl.InvTrxHdrId });
        }

        public IEnumerable<DialogItems> ConvertItemsToDialogItems(List<InvItem> invItems)
        {
            List<DialogItems> dialogItems = new List<DialogItems>();

            foreach (InvItem item in invItems)
            {
                dialogItems.Add(new DialogItems { 
                    Id = item.Id,
                    Name = item.Description,
                    Description = item.Remarks
                });
            }

            return dialogItems;
        }
    }
}

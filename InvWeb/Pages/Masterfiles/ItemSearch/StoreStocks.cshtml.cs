using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using CoreLib.Models.Inventory;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;
using CoreLib.DTO.InvItems;

namespace InvWeb.Pages.Masterfiles.StoreStock
{
    public class StoreStocksModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private ISearchServices services;

        public StoreStocksModel(ApplicationDbContext context)
        {
            _context = context;
            services = new SearchServices(_context);
        }

        public IList<InvItemStoreStocks> invItemStoreStocks { get;set; }

        public async Task OnGetAsync(int id)
        {
            invItemStoreStocks = new List<InvItemStoreStocks>();

            //get list of approved item details 
            var ApprovedItemDetails = await services.GetInvDetailsByIdAsync(id);

            foreach (var itemdtl in ApprovedItemDetails)
            {
                //simplify storeId
                var storeId = itemdtl.InvTrxHdr.InvStore.Id;

                //get item details by Id
                var itemDetails = _context.InvItems.Find(itemdtl.InvItemId);

                //check if item is not in the list
                var invDetailsCount = invItemStoreStocks.Where(c => c.Id == id 
                                             && c.InvTrxHdr.InvStoreId == storeId)
                                            .Count();

                //if not on the list, add to existing list
                if (invDetailsCount == 0)
                {
                    invItemStoreStocks.Add(new InvItemStoreStocks
                    {
                        Id = itemdtl.InvItemId,
                        InvItem_Code = itemDetails.Code,
                        InvItem_Description = itemDetails.Description,
                        Qty_Stock = services.GetAvailableCountByItem(id, itemdtl.InvTrxHdr.InvStoreId),
                        InvTrxHdr = itemdtl.InvTrxHdr,
                        InvUom = itemdtl.InvUom.uom
                        
                    });
                }
                
            }

        }

    }

}

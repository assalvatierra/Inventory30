using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;
using InvWeb.Data.Services;

namespace InvWeb.Pages.Masterfiles.StoreStock
{
    public class StoreStocksModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private SearchServices services;

        public StoreStocksModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
            services = new SearchServices(_context);
        }

        public IList<InvTrxDtl> InvTrxDtls { get;set; }

        public async Task OnGetAsync(int id)
        {
            InvTrxDtls = new List<InvTrxDtl>();

            //get list of approved item details 
            var ApprovedItemDetails = await services.GetInvDetailsByIdAsync(id);

            foreach (var itemdtl in ApprovedItemDetails)
            {
                //simplify storeId
                var storeId = itemdtl.InvTrxHdr.InvStore.Id;

                //get item details by Id
                var itemDetails = _context.InvItems.Find(itemdtl.InvItemId);

                //check if item is not in the list
                var invDetailsCount = InvTrxDtls.Where(c => c.Id == id 
                                             && c.InvTrxHdr.InvStoreId == storeId)
                                            .Count();

                //if not on the list, add to existing list
                if (invDetailsCount == 0)
                {
                    InvTrxDtls.Add(new InvTrxDtl
                    {
                        Id = itemdtl.InvItemId,
                        InvItem = itemDetails,
                        ItemQty = services.GetAvailableCountByItem(id, itemdtl.InvTrxHdr.InvStoreId),
                        InvTrxHdr = itemdtl.InvTrxHdr
                    });
                }
                
            }

        }

    }
}

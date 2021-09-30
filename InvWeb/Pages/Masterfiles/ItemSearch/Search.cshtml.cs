using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;
using WebDBSchema.Models.Items;

namespace InvWeb.Pages.Masterfiles.ItemSearch
{
    public class SearchModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public SearchModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ItemSearchResult> ItemSearchResults { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStr { get; set; }

        public async Task OnGetAsync()
        {
            ItemSearchResults = new List<ItemSearchResult>();
            var storeList = _context.InvStores.ToList();
            var itemList = _context.InvItems.ToList();
            var UomList = _context.InvUoms.ToList();

            //var invItemPerStore = _context.InvTrxDtls.Where(i=>i.InvTrxHdr.InvTrxHdrStatusId == 1);

            //var invItemPerStore = _context.InvTrxDtls
            //    .Where(i => i.InvTrxHdr.InvTrxHdrStatusId == 1)
            //    .GroupBy(x => new { x.InvTrxHdr.InvStoreId, x.InvItemId});

            var invItemPerStore = _context.InvTrxDtls
                .Include(c=>c.InvItem)
                .GroupBy(x=> new { x.InvItemId, x.InvTrxHdr.InvStoreId })
                .Select(p => new ItemSearchResult()
                                   {
                                       Id = p.Key.InvItemId,
                                       StoreId = p.Key.InvStoreId,
                                       Qty= p.Sum(x => x.ItemQty),
                });

          

            foreach(var item in await invItemPerStore.ToListAsync())
            {
              

                ItemSearchResults.Add(new ItemSearchResult { 
                    Id = item.Id,
                    InvStore = storeList.Where(s=>s.Id == item.StoreId).FirstOrDefault().StoreName,
                    Item = itemList.Where(s => s.Id == item.Id).FirstOrDefault().Description,
                    Qty = item.Qty,
                    Uom = itemList.Where(s => s.Id == item.Id).FirstOrDefault().InvUom.uom,

                });
            }

            if (!String.IsNullOrEmpty(SearchStr))
            {
                ItemSearchResults = ItemSearchResults.Where(c => c.Item.ToLower().Contains(SearchStr.ToLower())).ToList();
            }

        }

      



    }
}

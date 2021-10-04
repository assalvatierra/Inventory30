using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;

namespace InvWeb.Pages.Stores.Main
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InvStore InvStore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
          
            if (id == null)
            {
                return NotFound();
            }

            InvStore = await _context.InvStores
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvStore == null)
            {
                return NotFound();
            }

            ViewData["StoreId"] = id;
            ViewData["StoreInv"] = await GetInventory((int)id);

            return Page();
        }

        #region Services

        private readonly int TYPE_RECEIVED = 1;
        private readonly int TYPE_RELEASED = 2;

        private async Task<IEnumerable<StoreInvCount>> GetInventory(int storeId)
        {
            var invItems = await _context.InvItems.ToListAsync();

            //Todo: add filter to add only trx with approved status (statusId = 1) 
            var Received = await _context.InvTrxDtls
                .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_RECEIVED &&
                 h.InvTrxHdr.InvStoreId == storeId)
                .ToListAsync();

            var Released = await _context.InvTrxDtls
                .Where(h => h.InvTrxHdr.InvTrxTypeId == TYPE_RELEASED &&
                 h.InvTrxHdr.InvStoreId == storeId)
                .ToListAsync();

            List<StoreInvCount> storeInvItems = new();

            foreach (var item in invItems.Select(i=>i.Id))
            {
                int itemReceived = Received.Where(h => h.InvItemId == item).Sum(i => i.ItemQty);
                int itemReleased = Released.Where(h => h.InvItemId == item).Sum(i => i.ItemQty);

                if (Received.Where(h => h.InvItemId == item).Any())
                {
                    storeInvItems.Add(new StoreInvCount { 
                            Id = item,
                            Description = invItems.Where(i=>i.Id == item).FirstOrDefault().Description,
                            Count = (itemReceived - itemReleased)
                    });
                }

            }

            return storeInvItems;
        }

        #endregion

    }
}

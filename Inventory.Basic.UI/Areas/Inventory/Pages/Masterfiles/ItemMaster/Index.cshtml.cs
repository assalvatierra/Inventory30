using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Entities;
using Inventory.Basic;


namespace Inventory.Basic.UI.Masterfiles.ItemMaster
{
    public class IndexModel : PageModel
    {
        private readonly InvDbContext _context;

        public IndexModel(InvDbContext context)
        {
            _context = context;
        }

        public IList<InvItemIndexModel> InvItemIndex { get;set; }

        public async Task OnGetAsync()
        {
            InvItemIndex = new List<InvItemIndexModel>();
            var InvItem = await _context.InvItems
                .Include(i => i.InvUom).ToListAsync();

            foreach (var item in InvItem)
            {
                var invItemClassGroup = await _context.InvItemClasses.Where(i => i.InvItemId == item.Id).Include(i => i.InvClassification).ToListAsync();

                var invItemClass = new InvItemIndexModel
                {
                    InvItem = item,
                    InvItemClasses = invItemClassGroup
                };

                InvItemIndex.Add(invItemClass);
            }
        }
    }

    public class InvItemIndexModel
    {
        public InvItem InvItem { get; set; }
        public IEnumerable<InvItemClass> InvItemClasses { get;set; }
    }
}

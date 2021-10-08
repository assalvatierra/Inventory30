using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
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

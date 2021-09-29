using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSearch
{
    public class SearchModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public SearchModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvSupplierItem> InvSupplierItem { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchStr { get; set; }

        public async Task OnGetAsync()
        {
            var invSupItems = from sup in _context.InvSupplierItems select sup;
          
            if (!String.IsNullOrEmpty(SearchStr))
            {
                invSupItems = invSupItems.Where(c => c.InvItem.Description.Contains(SearchStr));
            }

            InvSupplierItem = await invSupItems
                .Include(i => i.InvItem)
                .Include(i => i.InvSupplier)
                .Include(i => i.InvItem.InvUom)
                .ToListAsync();
        }



    }
}

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
    public class ItemPricesModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public ItemPricesModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvSupplierItem> InvSupplierItem { get;set; }

        public async Task OnGetAsync(int id)
        {
            InvSupplierItem = await _context.InvSupplierItems
                .Where(i => i.InvItemId == id)
                .Include(i => i.InvItem)
                .Include(i => i.InvSupplier).ToListAsync();
        }
    }
}

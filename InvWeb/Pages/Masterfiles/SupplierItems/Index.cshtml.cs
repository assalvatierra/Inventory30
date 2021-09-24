using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.SupplierItems
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvSupplierItem> InvSupplierItem { get;set; }

        public async Task OnGetAsync()
        {
            InvSupplierItem = await _context.InvSupplierItems
                .Include(i => i.InvItem)
                .Include(i => i.InvSupplier).ToListAsync();
        }
    }
}

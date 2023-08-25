using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemPO
{
    [Authorize(Roles = "Admin,Purchaser")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvPoHdr> InvPoHdr { get;set; }

        public async Task OnGetAsync()
        {
            InvPoHdr = await _context.InvPoHdrs
                .Include(i => i.InvStore)
                .Include(i => i.InvSupplier).ToListAsync();
        }
    }
}

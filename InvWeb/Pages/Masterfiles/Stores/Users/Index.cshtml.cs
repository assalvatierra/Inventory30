using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.Stores.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvStoreUser> InvStoreUser { get;set; }

        public async Task OnGetAsync(int id)
        {
            InvStoreUser = await _context.InvStoreUsers
                .Where(i => i.InvStoreId == id)
                .Include(i => i.InvStore).ToListAsync();

            ViewData["storeId"] = id;
        }
    }
}

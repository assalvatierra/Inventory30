using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.Stores.Users
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
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

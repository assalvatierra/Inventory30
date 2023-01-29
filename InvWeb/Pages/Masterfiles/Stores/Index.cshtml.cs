using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace InvWeb.Pages.Stores
{
    [Authorize(Roles = "ADMIN,STORE")]
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvStore> InvStore { get;set; }

        public async Task OnGetAsync()
        {
            InvStore = await _context.InvStores.ToListAsync();
        }
    }
}

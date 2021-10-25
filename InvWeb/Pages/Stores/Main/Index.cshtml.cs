using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvWeb.Data;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using WebDBSchema.Models.Stores;
using Microsoft.AspNetCore.Authorization;

namespace InvWeb.Pages.Stores.Main
{
    [Authorize(Roles = "ADMIN,STORE")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly StoreServices _storeSvc;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
            _storeSvc = new StoreServices(context);
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
            ViewData["StoreInv"] = await _storeSvc.GetStoreItemsSummary((int)id);

            return Page();
        }
    }
}

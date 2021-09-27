using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;


namespace InvWeb.Pages.Stores.Main
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InvStore InvStore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvStore = await _context.InvStores.FirstOrDefaultAsync(m => m.Id == id);

            if (InvStore == null)
            {
                return NotFound();
            }

            ViewData["StoreId"] = id;
            return Page();
        }
    }
}

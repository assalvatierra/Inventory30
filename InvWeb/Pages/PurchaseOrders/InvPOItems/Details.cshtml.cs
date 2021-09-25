using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.PurchaseOrders.InvPOItems
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public InvPoItem InvPoItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoItem = await _context.InvPoItems
                .Include(i => i.InvItem)
                .Include(i => i.InvPoHdr)
                .Include(i => i.InvUom).FirstOrDefaultAsync(m => m.Id == id);

            if (InvPoItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

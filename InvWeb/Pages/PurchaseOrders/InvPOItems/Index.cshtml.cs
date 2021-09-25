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
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvPoItem> InvPoItem { get;set; }
       
        //Param: id = InvPOHdr.Id
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvPoItem = await _context.InvPoItems
                .Include(i => i.InvItem)
                .Include(i => i.InvPoHdr)
                .Include(i => i.InvUom).ToListAsync();

            ViewData["InvPoHdrId"] = id;

            return Page();
        }
    }
}

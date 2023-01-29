using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion.ConvItems
{
    public class DetailsModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public DetailsModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public InvUomConvItem InvUomConvItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvUomConvItem = await _context.InvUomConvItems
                .Include(i => i.InvClassification)
                .Include(i => i.InvItem)
                .Include(i => i.InvUomConversion).FirstOrDefaultAsync(m => m.Id == id);

            if (InvUomConvItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

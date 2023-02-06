using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.Extensions.Logging;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster.WarningLevels
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public DeleteModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InvWarningLevel InvWarningLevel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvWarningLevel = await _context.InvWarningLevels
                .Include(i => i.InvItem)
                .Include(i => i.InvUom)
                .Include(i => i.InvWarningType).FirstOrDefaultAsync(m => m.Id == id);

            if (InvWarningLevel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {

                if (id == null)
                {
                    return NotFound();
                }

                InvWarningLevel = await _context.InvWarningLevels.FindAsync(id);

                var invid = InvWarningLevel.InvItemId;

                if (InvWarningLevel != null)
                {
                    _context.InvWarningLevels.Remove(InvWarningLevel);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("../Details", new { id = invid });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }
    }
}

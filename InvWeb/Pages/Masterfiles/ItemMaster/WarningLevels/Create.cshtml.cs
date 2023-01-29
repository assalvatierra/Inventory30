using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using Microsoft.Extensions.Logging;

namespace InvWeb.Pages.Masterfiles.ItemMaster.WarningLevels
{
    public class CreateModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public CreateModel(ILogger<IndexModel> logger, InvWeb.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["InvItemId"] = new SelectList(_context.InvItems
                .Select(i=> new { i.Id, Name = String.Format("{0} {1} {2}", i.Code, i.Description, i.Remarks)}), "Id", "Name");
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["InvWarningTypeId"] = new SelectList(_context.InvWarningTypes, "Id", "Desc");
            return Page();
        }

        [BindProperty]
        public InvWarningLevel InvWarningLevel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                _context.InvWarningLevels.Add(InvWarningLevel);
                await _context.SaveChangesAsync();

                return RedirectToPage("../Details", new { id = InvWarningLevel.InvItemId });

            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Page();
            }
        }
    }
}

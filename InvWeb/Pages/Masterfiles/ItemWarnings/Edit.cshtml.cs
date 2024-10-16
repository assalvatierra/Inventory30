﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemWarnings
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
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
            ViewData["InvItemId"] = new SelectList(
                      _context.InvItems.Select(x => new {
                          Name = String.Format("{0} - {1} {2}", x.Code, x.Description, x.Remarks),
                          Id = x.Id
                      }), "Id", "Name");
            ViewData["InvUomId"] = new SelectList(_context.InvUoms, "Id", "uom");
            ViewData["InvWarningTypeId"] = new SelectList(_context.Set<InvWarningType>(), "Id", "Desc");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(InvWarningLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvWarningLevelExists(InvWarningLevel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InvWarningLevelExists(int id)
        {
            return _context.InvWarningLevels.Any(e => e.Id == id);
        }
    }
}

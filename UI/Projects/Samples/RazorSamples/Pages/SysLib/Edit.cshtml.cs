using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.SysDB;
using RazorSamples.Data;

namespace RazorSamples.Pages.SysLib
{
    public class EditModel : PageModel
    {
        private readonly RazorSamples.Data.RazorSamplesContext _context;

        public EditModel(RazorSamples.Data.RazorSamplesContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SysService SysService { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SysService == null)
            {
                return NotFound();
            }

            var sysservice =  await _context.SysService.FirstOrDefaultAsync(m => m.Id == id);
            if (sysservice == null)
            {
                return NotFound();
            }
            SysService = sysservice;
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

            _context.Attach(SysService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SysServiceExists(SysService.Id))
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

        private bool SysServiceExists(int id)
        {
          return _context.SysService.Any(e => e.Id == id);
        }
    }
}

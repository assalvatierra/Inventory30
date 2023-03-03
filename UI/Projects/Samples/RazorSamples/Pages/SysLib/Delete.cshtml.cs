using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.SysDB;
using RazorSamples.Data;

namespace RazorSamples.Pages.SysLib
{
    public class DeleteModel : PageModel
    {
        private readonly RazorSamples.Data.RazorSamplesContext _context;

        public DeleteModel(RazorSamples.Data.RazorSamplesContext context)
        {
            _context = context;
        }

        [BindProperty]
      public SysService SysService { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SysService == null)
            {
                return NotFound();
            }

            var sysservice = await _context.SysService.FirstOrDefaultAsync(m => m.Id == id);

            if (sysservice == null)
            {
                return NotFound();
            }
            else 
            {
                SysService = sysservice;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SysService == null)
            {
                return NotFound();
            }
            var sysservice = await _context.SysService.FindAsync(id);

            if (sysservice != null)
            {
                SysService = sysservice;
                _context.SysService.Remove(SysService);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

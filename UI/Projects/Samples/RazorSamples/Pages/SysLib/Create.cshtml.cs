using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Models.SysDB;
using RazorSamples.Data;

namespace RazorSamples.Pages.SysLib
{
    public class CreateModel : PageModel
    {
        private readonly RazorSamples.Data.RazorSamplesContext _context;

        public CreateModel(RazorSamples.Data.RazorSamplesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SysService SysService { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SysService.Add(SysService);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

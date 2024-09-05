using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelOrigins
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SteelOrigin SteelOrigin { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.SteelOrigins == null || SteelOrigin == null)
            {
                return Page();
            }

            _context.SteelOrigins.Add(SteelOrigin);
            await _context.SaveChangesAsync();

            await CreateInvItemOriginAsync();

            return RedirectToPage("./Index");
        }

        public async Task CreateInvItemOriginAsync()
        {

            InvItemOrigin invItemOrigin = new InvItemOrigin();
            invItemOrigin.Name = SteelOrigin.Name;
            invItemOrigin.Code = SteelOrigin.Code;

            _context.InvItemOrigins.Add(invItemOrigin);
            await _context.SaveChangesAsync();
        }
    }
}

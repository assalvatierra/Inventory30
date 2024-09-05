using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelBrands {
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
        public SteelBrand SteelBrand { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.SteelBrands == null || SteelBrand == null)
            {
                return Page();
            }

            _context.SteelBrands.Add(SteelBrand);
            await _context.SaveChangesAsync();

            await CreateInvItemBrandAsync();

            return RedirectToPage("./Index");
        }

        public async Task CreateInvItemBrandAsync()
        {

            InvItemBrand invItemBrand = new InvItemBrand();
            invItemBrand.Name = SteelBrand.Name;
            invItemBrand.Code = SteelBrand.Code;

            _context.InvItemBrands.Add(invItemBrand);
            await _context.SaveChangesAsync();
        }
    }
}

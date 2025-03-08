using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace InvWeb.Pages.Masterfiles.InvItemMasters
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
            var invitems = _context.InvItems.Count();
            //ViewData["InvItemId"] = new SelectList(
            //       _context.InvItems.Select(x => new
            //       {
            //           Name = String.Format("{0} - {1} {2}", x.Code, x.Description, x.Remarks),
            //           Id = x.Id
            //       }), "Id", "Name");

            ViewData["InvItemId"] = new SelectList(
                   _context.InvItems.Where(c => c.Code != null).Select(x => new
                   {
                       Name = String.Format("{0} {1} {2} {3} {4} {5}", x.InvItemSpec_Steel.First().SteelMainCat.Name, x.InvItemSpec_Steel.First().SteelSubCat.Name, 
                       x.InvItemSpec_Steel.First().SteelSize.Name, x.InvItemSpec_Steel.First().SteelBrand.Name, 
                       x.InvItemSpec_Steel.First().SteelMaterialGrade.Name, x.InvItemSpec_Steel.First().SteelMaterial.Name),
                       Id = x.Id
                   }), "Id", "Name");

            ViewData["InvItemBrandId"] = new SelectList(_context.InvItemBrands.OrderBy(s => s.Name), "Id", "Name");
            ViewData["InvItemOriginId"] = new SelectList(_context.InvItemOrigins.OrderBy(s => s.Name), "Id", "Name");
            ViewData["InvUomId"] = new SelectList(_context.InvUoms.OrderBy(s => s.uom), "Id", "uom");
            return Page();
        }

        [BindProperty]
        public InvItemMaster InvItemMaster { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.InvItemMasters == null || InvItemMaster == null)
            {
                return Page();
            }

            _context.InvItemMasters.Add(InvItemMaster);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

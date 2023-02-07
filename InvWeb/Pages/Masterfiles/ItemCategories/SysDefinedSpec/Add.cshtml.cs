using System;
using CoreLib.Inventory.Interfaces;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemCategories.SysDefinedSpec
{
    public class AddModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly IItemSpecServices _itemSpecServices;

        public AddModel(ApplicationDbContext context, ILogger<AddModel> logger)
        {
            _context = context;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }

        public IActionResult OnGet(int id)
        {
            ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Description", id);

            //ViewData["InvItemSysDefinedSpecsId"] = _itemSpecServices.GetDefindSpecsSelectList();
            ViewData["InvItemSysDefinedSpecsId"] =
                new SelectList(_itemSpecServices.GetDefinedSpecs().Select(x => new
                {
                    Name = String.Format("({0}) {1} {2}", x.SpecCode, x.SpecName, x.SpecGroup),
                    Value = x.Id
                }), "Value", "Name");

            ViewData["SysDefinedSpecsList"] = _context.InvItemSysDefinedSpecs.ToList();
            ViewData["CategoryId"] = id;
            return Page();
        }

        [BindProperty]
        public InvCategorySpecDef InvCategorySpecDefs { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvCategorySpecDefs.Add(InvCategorySpecDefs);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }

    }
}

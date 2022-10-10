using InvWeb.Data.Interfaces;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemCategories.SysDefinedSpec
{
    public class AddModel : PageModel
    {

        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly IItemSpecServices _itemSpecServices;

        public AddModel(InvWeb.Data.ApplicationDbContext context, ILogger<AddModel> logger)
        {
            _context = context;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }

        public IActionResult OnGet(int id)
        {
            ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Description", id);
            ViewData["InvItemSysDefinedSpecsId"] = _itemSpecServices.GetDefindSpecsSelectList();
            ViewData["SysDefinedSpecsList"] = _context.InvItemSysDefinedSpecs.ToList();
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

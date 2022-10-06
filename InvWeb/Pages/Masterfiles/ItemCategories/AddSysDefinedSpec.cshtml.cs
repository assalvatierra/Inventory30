using InvWeb.Data.Interfaces;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemCategories
{
    public class AddSysDefinedSpecModel : PageModel
    {

        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly IItemSpecServices _itemSpecServices;

        public AddSysDefinedSpecModel(InvWeb.Data.ApplicationDbContext context, ILogger<AddSysDefinedSpecModel> logger)
        {
            _context = context;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }

        public IActionResult OnGet(int id)
        {
            ViewData["InvCategoryId"] = new SelectList(_context.InvCategories, "Id", "Description", id);
            ViewData["InvItemSysDefinedSpecsId"] = _itemSpecServices.GetDefindSpecsSelectList();
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

            return RedirectToPage("./Index");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.Extensions.Logging;
using InvWeb.Data.Services;
using NuGet.Versioning;
using CoreLib.Models.Inventory;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class CreateCopyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemSpecServices _itemSpecServices;

        public CreateCopyModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemSpecServices = new ItemSpecServices(context, logger);

        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        [BindProperty]
        public Boolean showSpec { get; set; }

        public async Task<IActionResult> OnGetAsync( int? id, int? categoryId)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItem = await _context.InvItems
                .Include(i => i.InvCategory)
                .Include(i => i.InvItemSpec_Steel)
                .Include(i => i.InvUom)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelMainCat)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelSubCat)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelMaterial)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelMaterialGrade)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelOrigin)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelBrand)
                .Include(i => i.InvItemSpec_Steel)
                    .ThenInclude(i => i.SteelSize)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (InvItem == null)
            {
                return NotFound();
            }

            if (categoryId != null)
            {
                InvItem.InvCategoryId = (int)categoryId;
            }

            if (InvItem.InvItemSpec_Steel != null)
            {
                InvItemSpec_Steel = InvItem.InvItemSpec_Steel.FirstOrDefault();
            }
            

            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description");
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
            //Steel Specifications
            ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats, "Id", "Name", InvItemSpec_Steel);
            ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats, "Id", "Name");
            ViewData["SteelBrands"] = new SelectList(_context.SteelBrands, "Id", "Name");
            ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins, "Id", "Name");
            ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials, "Id", "Name");
            ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades, "Id", "Name");
            ViewData["SteelSizes"] = new SelectList(_context.SteelSizes, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.InvItems.Add(InvItem);

            await _context.SaveChangesAsync();

            await AddInvItemSteel();

            return RedirectToPage("./Index");
        }

        public async Task AddInvItemSteel()
        {
            InvItemSpec_Steel.InvItemId = InvItem.Id;

            _context.InvItemSpec_Steel.Add(InvItemSpec_Steel);

            await _context.SaveChangesAsync();
        }

        private bool InvItemExists(int id)
        {
            return _context.InvItems.Any(e => e.Id == id);
        }

    }
}

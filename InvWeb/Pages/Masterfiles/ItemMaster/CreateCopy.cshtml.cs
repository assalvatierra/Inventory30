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

            ViewData["ErrorMsg"] = "";
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

            InvItem.Id = 0;

            if (CheckIfItemExists(InvItemSpec_Steel))
            {
                ModelState.AddModelError("ErrorDuplicateItem", "Brand/Origin already exist in the system");

                InvItem = InvItem;
                InvItemSpec_Steel = InvItemSpec_Steel;

                ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description", InvItem.InvCategoryId);
                ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom", InvItem.InvUomId);

                //Steel Specifications
                ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelMainCatId);
                ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelSubCatId);
                ViewData["SteelBrands"] = new SelectList(_context.SteelBrands.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelBrandId);
                ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelOriginId);
                ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelMaterialId);
                ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelMaterialGradeId);
                ViewData["SteelSizes"] = new SelectList(_context.SteelSizes.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelSizeId);

                //return RedirectToAction("/CreateCopy", new { id = InvItem.Id });
                return Page();
            }


            _context.InvItems.Add(InvItem);

            await _context.SaveChangesAsync();

            await AddInvItemSteel();

            return RedirectToPage("./Index");
        }

        public async Task AddInvItemSteel()
        {
            InvItemSpec_Steel.Id = 0;
            InvItemSpec_Steel.InvItemId = InvItem.Id;

            _context.InvItemSpec_Steel.Add(InvItemSpec_Steel);

            await _context.SaveChangesAsync();
        }

        private bool InvItemExists(int id)
        {
            return _context.InvItems.Any(e => e.Id == id);
        }

        private bool CheckIfItemExists(InvItemSpec_Steel itemSpec)
        {

            var resultExists = _context.InvItemSpec_Steel.Where(i =>
                i.SteelBrandId == itemSpec.SteelBrandId
                && i.SteelOriginId == itemSpec.SteelOriginId
                && i.SteelMaterialId == itemSpec.SteelMaterialId
                && i.SteelMaterialGradeId == itemSpec.SteelMaterialGradeId
                && i.SteelSizeId == itemSpec.SteelSizeId
                && i.SteelMainCatId == itemSpec.SteelMainCatId
                && i.SteelSubCatId == itemSpec.SteelSubCatId).Any();

            if (resultExists)
            {
                return true;
            }

            return false;

        }

    }
}

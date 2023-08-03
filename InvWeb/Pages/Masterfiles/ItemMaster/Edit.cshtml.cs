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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemSpecServices _itemSpecServices;

        public EditModel(ApplicationDbContext context, ILogger<CreateModel> logger)
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
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //ModelState.Remove("InvItemSpec_Steel.Id");

            if (!ModelState.IsValid)
            {
                ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description");
                ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");
                //Steel Specifications
                ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats, "Id", "Name");
                ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats, "Id", "Name");
                ViewData["SteelBrands"] = new SelectList(_context.SteelBrands, "Id", "Name");
                ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins, "Id", "Name");
                ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials, "Id", "Name");
                ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades, "Id", "Name");
                return Page();
            }

            _context.Attach(InvItem).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvItemExists(InvItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError("ItemsMasters Edit: Unable to update at OnPostAsync itemId:" + InvItem.Id);
                    throw;
                }
            }

            await UpdateSteelSpecs();

            return RedirectToPage("./Index");
        }

        private bool InvItemExists(int id)
        {
            return _context.InvItems.Any(e => e.Id == id);
        }

        private async Task UpdateSteelSpecs()
        {
            InvItemSpec_Steel.InvItemId = InvItem.Id;

            _context.Attach(InvItemSpec_Steel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               
                _logger.LogError("UpdateSteelSpecs Edit: Unable to update at OnPostAsync InvItemSpec_SteelId:" + InvItem.Id);
                throw;
            }
        }

    }
}

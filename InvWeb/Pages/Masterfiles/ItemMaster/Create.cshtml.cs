using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreLib.Inventory.Models;
using InvWeb.Data.Services;
using Microsoft.Extensions.Logging;
using CoreLib.Models.Inventory;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IItemSpecServices _itemSpecServices;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }

        public IActionResult OnGet(string desc, string code, int? categoryId, string remarks, int? uomId)
        {

            //load InvItem on page reload 
            if (!String.IsNullOrEmpty(code))
            {
                InvItem = new InvItem();
                InvItem.Description = desc;
                InvItem.Code = code;
                InvItem.Remarks = remarks;

                if (categoryId != null)
                {
                    InvItem.InvCategoryId = (int)categoryId;
                }

                if (uomId != null)
                {
                    InvItem.InvUomId = (int)uomId;
                }
            }

            //set initial category to default = 1
            if (categoryId == null)
            {
                categoryId = 1;
            }

            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description", categoryId);
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom");

            //Steel Specifications
            //ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats.OrderBy(s=>s.Name), "Id", "Name");
            //ViewData["SteelSubCats"]  = new SelectList(_context.SteelSubCats.OrderBy(s => s.Name), "Id", "Name");
            //ViewData["SteelBrands"]   = new SelectList(_context.SteelBrands.OrderBy(s => s.Name), "Id", "Name");
            //ViewData["SteelOrigins"]  = new SelectList(_context.SteelOrigins.OrderBy(s => s.Name), "Id", "Name");
            //ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials.OrderBy(s => s.Name), "Id", "Name");
            //ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades.OrderBy(s => s.Name), "Id", "Name");
            //ViewData["SteelSizes"] = new SelectList(_context.SteelSizes.OrderBy(s => s.Name), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public InvItem InvItem { get; set; }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            InvItem = InvItem;


            ViewData["InvCategoryId"] = new SelectList(_context.Set<InvCategory>(), "Id", "Description", InvItem.InvCategoryId);
            ViewData["InvUomId"] = new SelectList(_context.Set<InvUom>(), "Id", "uom", InvItem.InvUomId);

                //Steel Specifications
                //ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelMainCatId);
                //ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelSubCatId);
                //ViewData["SteelBrands"] = new SelectList(_context.SteelBrands.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelBrandId);
                //ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelOriginId);
                //ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelMaterialId);
                //ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelMaterialGradeId);
                //ViewData["SteelSizes"] = new SelectList(_context.SteelSizes.OrderBy(s => s.Name), "Id", "Name", InvItemSpec_Steel.SteelSizeId);

                //return Page();
            

            _context.InvItems.Add(InvItem);

            await _context.SaveChangesAsync();

            //await AddInvItemSteel();

            return RedirectToPage("./Index");
        }

        //public async Task AddInvItemSteel()
        //{
        //    InvItemSpec_Steel.InvItemId = InvItem.Id;

        //    _context.InvItemSpec_Steel.Add(InvItemSpec_Steel);

        //    await _context.SaveChangesAsync();
        //}


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

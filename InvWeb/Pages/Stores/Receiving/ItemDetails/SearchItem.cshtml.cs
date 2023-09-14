using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InvWeb.Data.Services;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;
using CoreLib.DTO.Receiving;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Releasing;

namespace InvWeb.Pages.Stores.Receiving.ItemDetails
{
    public class SearchItemModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchItemModel> _logger;
        private readonly IItemSpecServices _itemSpecServices;

        public SearchItemModel(ApplicationDbContext context, ILogger<SearchItemModel> logger)
        {
            _context = context;
            _logger = logger;
            _itemSpecServices = new ItemSpecServices(context, logger);
        }

        public IActionResult OnGet(int id, string actionType)
        {

            //Steel Specifications
            ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats, "Id", "Name");
            ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats, "Id", "Name");
            ViewData["SteelBrands"] = new SelectList(_context.SteelBrands, "Id", "Name");
            ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins, "Id", "Name");
            ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials, "Id", "Name");
            ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades, "Id", "Name");
            ViewData["SteelSizes"] = new SelectList(_context.SteelSizes, "Id", "Name");

            ItemList = new List<InvItem>();

            HdrId = id;
            ViewData["HdrId"] = id;
            ViewData["ActionType"] = actionType;

            return Page();
        }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        public List<InvItem> ItemList { get; set; }

        [BindProperty]
        public int HdrId { get; set; }

        [BindProperty]
        public string ActionType { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Steel Specifications
            ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats, "Id", "Name");
            ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats, "Id", "Name");
            ViewData["SteelBrands"] = new SelectList(_context.SteelBrands, "Id", "Name");
            ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins, "Id", "Name");
            ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials, "Id", "Name");
            ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades, "Id", "Name");
            ViewData["SteelSizes"] = new SelectList(_context.SteelSizes, "Id", "Name");


            var ItemListResult = await _context.InvItemSpec_Steel
                .Include(c=>c.InvItem)
                .Where(c => c.SteelMainCatId == InvItemSpec_Steel.SteelMainCatId 
                         && c.SteelSubCatId == InvItemSpec_Steel.SteelSubCatId
                         && c.SteelBrandId == InvItemSpec_Steel.SteelBrandId
                         && c.SteelOriginId == InvItemSpec_Steel.SteelOriginId
                         && c.SteelMaterialId == InvItemSpec_Steel.SteelMaterialId
                         && c.SteelMaterialGradeId == InvItemSpec_Steel.SteelMaterialGradeId
                         && c.SteelSizeId == InvItemSpec_Steel.SteelSizeId)
                .Select(c=>c.InvItem).ToListAsync();

            ItemList = ItemListResult;

            ViewData["HdrId"] = HdrId;
            ViewData["ActionType"] = ActionType;

            return Page();
        }
    }
}

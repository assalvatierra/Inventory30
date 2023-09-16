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

namespace InvWeb.Pages.Stores.Releasing.ItemDetails
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

        public IActionResult OnGet(int id, string actionType, int? itemDetailsId)
        {
            int SteelMainCatId = 0;
            int SteelSubCatId = 0;
            int SteelBrandId = 0;
            int SteelOriginId = 0;
            int SteelMaterialId = 0;
            int SteelMaterialGradeId = 0;
            int SteelSizeId = 0;

            if (string.IsNullOrEmpty(actionType))
            {
                actionType = "Create";
            }

            if (itemDetailsId == null)
            {
                itemDetailsId = 0;
            }

            if (actionType == "Edit" && itemDetailsId != null)
            {
                var selectedItem = _context.InvTrxDtls
                    .Include(i=>i.InvItem)
                    .ThenInclude(i=>i.InvItemSpec_Steel)
                    .Where(i=>i.Id == itemDetailsId).FirstOrDefault();

                if (selectedItem.InvItem.InvItemSpec_Steel != null)
                {
                    var itemSpecs = selectedItem.InvItem.InvItemSpec_Steel.First();

                    SteelMainCatId = itemSpecs.SteelMainCatId;
                    SteelSubCatId = itemSpecs.SteelSubCatId;
                    SteelBrandId = itemSpecs.SteelBrandId;
                    SteelOriginId = itemSpecs.SteelOriginId;
                    SteelMaterialId = itemSpecs.SteelMaterialId;
                    SteelMaterialGradeId = itemSpecs.SteelMaterialGradeId;
                    SteelSizeId = itemSpecs.SteelSizeId;


                    var ItemListResult = _context.InvItemSpec_Steel
                        .Include(c => c.InvItem)
                        .Where(c => c.SteelMainCatId == SteelMainCatId
                                 && c.SteelSubCatId == SteelSubCatId
                                 && c.SteelBrandId == SteelBrandId
                                 && c.SteelOriginId == SteelOriginId
                                 && c.SteelMaterialId == SteelMaterialId
                                 && c.SteelMaterialGradeId == SteelMaterialGradeId
                                 && c.SteelSizeId == SteelSizeId)
                        .Select(c => c.InvItem).ToList();

                    if (ItemListResult == null)
                    {
                        return RedirectToAction("Create");
                    }

                    ItemList = ItemListResult;
                }
            }

            if (ItemList == null)
            {
                ItemList = new List<InvItem>();
            }

            //Steel Specifications
            ViewData["SteelMainCats"] = new SelectList(_context.SteelMainCats, "Id", "Name", SteelMainCatId);
            ViewData["SteelSubCats"] = new SelectList(_context.SteelSubCats, "Id", "Name", SteelSubCatId);
            ViewData["SteelBrands"] = new SelectList(_context.SteelBrands, "Id", "Name", SteelBrandId);
            ViewData["SteelOrigins"] = new SelectList(_context.SteelOrigins, "Id", "Name", SteelOriginId);
            ViewData["SteelMaterials"] = new SelectList(_context.SteelMaterials, "Id", "Name", SteelMaterialId);
            ViewData["SteelMaterialGrades"] = new SelectList(_context.SteelMaterialGrades, "Id", "Name", SteelMaterialGradeId);
            ViewData["SteelSizes"] = new SelectList(_context.SteelSizes, "Id", "Name", SteelSizeId);


            HdrId = id;
            ViewData["HdrId"] = id;
            ViewData["ItemDetailsId"] = itemDetailsId;
            ViewData["ActionType"] = actionType;

            return Page();
        }

        [BindProperty]
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }

        public List<InvItem> ItemList { get; set; }

        [BindProperty]
        public int HdrId { get; set; }

        [BindProperty]
        public int ItemDetailsId { get; set; }

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

            if (ItemListResult == null)
            {
                return RedirectToAction("Create");
            }

            ItemList = ItemListResult;

            ViewData["HdrId"] = HdrId;
            ViewData["ActionType"] = ActionType;
            ViewData["ItemDetailsId"] = ItemDetailsId;

            return Page();
        }
    }
}

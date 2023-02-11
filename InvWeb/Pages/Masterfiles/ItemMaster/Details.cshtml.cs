using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InvItem InvItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvItem = await _context.InvItems
                .Include(i => i.InvUom)
                .Include(i => i.InvItemClasses)
                .Include(i => i.InvWarningLevels)
                    .ThenInclude(i => i.InvWarningType)
                .Include(i=> i.InvItemSpec_Steel)
                .Include(i => i.InvItemCustomSpecs)
                    .ThenInclude(i => i.InvCustomSpec)
                .Include(i => i.InvCategory)
                    .ThenInclude(i => i.InvCatCustomSpecs)
                    .ThenInclude(i => i.InvCustomSpec)
                .Include(i => i.InvCategory)
                    .ThenInclude(i =>i. InvCategorySpecDefs)
                    .ThenInclude(i =>i.InvItemSysDefinedSpec)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (InvItem == null)
            {
                return NotFound();
            }


            ViewData["InvItemClass"] = await _context.InvItemClasses
                .Where(i => i.InvItemId == id)
                .Include(i => i.InvClassification)
                .ToListAsync();

            ViewData["IsSteelSpec"] = IsItemCategorySpecSteel();
            ViewData["SteelSpecDetails"] = GetInvItemSpec_SteelDetails();
            ViewData["ItemCustomSpecs"] = GetItemCustomSpecList();
            ViewData["ItemCategoryCustomSpecs"] = GetItemCategoryCustomSpecList();

            return Page();
        }

        private bool IsItemCategorySpecSteel()
        {
            return InvItem.InvCategory.InvCategorySpecDefs
                 .Where(i => i.InvItemSysDefinedSpec.SpecCode == "002")
                 .Any();
        }

        private InvItemSpec_Steel GetInvItemSpec_SteelDetails()
        {
            var specSteel = InvItem.InvItemSpec_Steel;

            if (specSteel == null)
            {
                return null;
            }

            return specSteel.FirstOrDefault();
        }

        private List<InvItemCustomSpec> GetItemCustomSpecList()
        {
            var catcustomSpecs = InvItem.InvCategory.InvCatCustomSpecs
                                    .Select(c => c.InvCustomSpecId)
                                    .ToList();
            var customSpecs_NotIn_CategoryCustomSpecs = InvItem.InvItemCustomSpecs
                                                        .Where(s => !catcustomSpecs.Contains(s.InvCustomSpecId))
                                                        .ToList();

            return customSpecs_NotIn_CategoryCustomSpecs;
        }

        private List<InvItemCustomSpec> GetItemCategoryCustomSpecList()
        {
            var catcustomSpecs = InvItem.InvCategory.InvCatCustomSpecs
                                    .Select(c => c.InvCustomSpecId)
                                    .ToList();
            var customSpecs_NotIn_CategoryCustomSpecs = InvItem.InvItemCustomSpecs
                                                        .Where(s => !catcustomSpecs.Contains(s.InvCustomSpecId))
                                                        .ToList();

            return customSpecs_NotIn_CategoryCustomSpecs;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<InvItemIndexModel> InvItemIndex { get;set; }

        public async Task OnGetAsync()
        {
            InvItemIndex = new List<InvItemIndexModel>();
            var InvItem = await _context.InvItems
                .Include(i => i.InvCategory)
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
                .Include(i => i.InvUom)
                .Include(i => i.InvItemCustomSpecs)
                    .ThenInclude(i=>i.InvCustomSpec)
                .Include(i => i.InvWarningLevels)
                    .ThenInclude(i => i.InvWarningType)
                .Include(i => i.InvWarningLevels)
                    .ThenInclude(i => i.InvUom)
                .OrderBy(s=>s.Code)
                    .ThenBy(s2=>s2.Description)
                .ToListAsync();

            foreach (var item in InvItem)
            {
                var invItemClassGroup = item.InvItemClasses.ToList();

                var invItemSpecSteel = item.InvItemSpec_Steel.FirstOrDefault();

                var invItemClass = new InvItemIndexModel
                {
                    InvItem = item,
                    InvItemClasses = invItemClassGroup,
                    InvItemSpec_Steel = invItemSpecSteel
                };

                InvItemIndex.Add(invItemClass);
            }
        }
    }

    public class InvItemIndexModel
    {
        public InvItem InvItem { get; set; }
        public InvItemSpec_Steel InvItemSpec_Steel { get; set; }
        public IEnumerable<InvItemClass> InvItemClasses { get;set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using CoreLib.Inventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace InvWeb.Pages.Masterfiles.ItemMaster
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, InvWeb.Data.ApplicationDbContext context)
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
                var invItemClassGroup = await _context.InvItemClasses
                    .Where(i => i.InvItemId == item.Id)
                    .Include(i => i.InvClassification)
                    .ToListAsync();

                var invItemClass = new InvItemIndexModel
                {
                    InvItem = item,
                    InvItemClasses = invItemClassGroup
                };

                InvItemIndex.Add(invItemClass);
            }
        }
    }

    public class InvItemIndexModel
    {
        public InvItem InvItem { get; set; }
        public IEnumerable<InvItemClass> InvItemClasses { get;set; }
    }
}

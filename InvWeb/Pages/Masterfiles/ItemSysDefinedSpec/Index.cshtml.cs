using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec
{
    public class IndexModel : PageModel
    {
       
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemSysDefinedSpecs> InvItemSysDefinedSpecs { get; set; }

        public async Task OnGetAsync()
        {
            InvItemSysDefinedSpecs = await _context.InvItemSysDefinedSpecs
                .Include(i => i.InvCategorySpecDefs)
                    .ThenInclude(i => i.InvCategory)
                .ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemMaster.Categories
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvCategory> InvCategory { get;set; }

        public async Task OnGetAsync()
        {
            InvCategory = await _context.InvCategories
                .Include(i=>i.InvCategorySpecDefs)
                    .ThenInclude(i=>i.InvItemSysDefinedSpec)
                .Include(i => i.InvCatCustomSpecs)
                .ToListAsync();
        }
    }
}

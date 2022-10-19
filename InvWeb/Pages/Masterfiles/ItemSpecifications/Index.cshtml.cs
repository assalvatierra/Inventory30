using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecifications
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemCustomSpec> InvItemCustomSpec { get;set; }

        public async Task OnGetAsync()
        {
            InvItemCustomSpec = await _context.InvItemCustomSpecs
                .Include(i => i.InvCustomSpec)
                .Include(i => i.InvItem).ToListAsync();
        }
    }
}

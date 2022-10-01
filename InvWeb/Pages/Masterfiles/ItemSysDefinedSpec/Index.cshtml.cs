using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemSysDefinedSpec
{
    public class IndexModel : PageModel
    {
       
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemSysDefinedSpecs> InvItemSysDefinedSpecs { get; set; }

        public async Task OnGetAsync()
        {
            InvItemSysDefinedSpecs = await _context.InvItemSysDefinedSpecs
                .Include(i=>i.InvCategory)
                .ToListAsync();
        }
    }
}

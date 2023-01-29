using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpec_Steel
{
    public class IndexModel : PageModel
    {

        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvItemSpec_Steel> InvItemSpec_Steel { get; set; }

        public async Task OnGetAsync()
        {
            InvItemSpec_Steel = await _context.InvItemSpec_Steel
                .Include(i => i.InvItem)
                .ToListAsync();
        }
    }
}

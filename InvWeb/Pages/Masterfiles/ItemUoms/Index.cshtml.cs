using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;
using Microsoft.AspNetCore.Authorization;

namespace InvWeb.Pages.Masterfiles.ItemUoms
{

    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvUom> InvUom { get;set; }

        public async Task OnGetAsync()
        {
            InvUom = await _context.InvUoms.ToListAsync();
        }
    }
}

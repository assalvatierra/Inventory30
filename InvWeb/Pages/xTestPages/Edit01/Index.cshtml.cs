using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.xTestPages.Edit01
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvCategory> InvCategory { get;set; }

        //public async Task OnGetAsync()
        //{
        //    InvCategory = await _context.InvCategories.ToListAsync();
        //}

        public void OnGet()
        {
            InvCategory = _context.InvCategories.FromSqlRaw<InvCategory>("select * from dbo.InvCategories;").ToList();

        }
    }
}

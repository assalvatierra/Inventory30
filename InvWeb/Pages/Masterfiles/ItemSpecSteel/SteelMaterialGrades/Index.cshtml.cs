using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Models;

namespace InvWeb.Pages.Masterfiles.ItemSpecSteel.SteelMaterialGrades
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SteelMaterialGrade> SteelMaterialGrade { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SteelMaterialGrades != null)
            {
                SteelMaterialGrade = await _context.SteelMaterialGrades.ToListAsync();
            }
        }
    }
}

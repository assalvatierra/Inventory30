using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Models.SysDB;
using RazorSamples.Data;

namespace RazorSamples.Pages.SysLib
{
    public class IndexModel : PageModel
    {
        private readonly RazorSamples.Data.RazorSamplesContext _context;

        public IndexModel(RazorSamples.Data.RazorSamplesContext context)
        {
            _context = context;
        }

        public IList<SysService> SysService { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SysService != null)
            {
                SysService = await _context.SysService.ToListAsync();
            }
        }
    }
}

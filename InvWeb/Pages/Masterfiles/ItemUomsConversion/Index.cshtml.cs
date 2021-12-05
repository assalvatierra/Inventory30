using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvUomConversion> InvUomConversion { get;set; }

        public async Task OnGetAsync()
        {
            try
            {
                InvUomConversion = await _context.InvUomConversions.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}

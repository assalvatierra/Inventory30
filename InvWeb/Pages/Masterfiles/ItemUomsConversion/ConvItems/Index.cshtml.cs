using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InvWeb.Data;
using WebDBSchema.Models;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion.ConvItems
{
    public class IndexModel : PageModel
    {
        private readonly InvWeb.Data.ApplicationDbContext _context;

        public IndexModel(InvWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<InvUomConvItem> InvUomConvItem { get;set; }

        public async Task OnGetAsync(int id)
        {
            try
            {

                InvUomConvItem = await _context.InvUomConvItems      
                    .Include(i => i.InvClassification)
                    .Include(i => i.InvItem)
                    .Include(i => i.InvUomConversion)
                    .Where(c=>c.InvUomConversionId == id)
                    .ToListAsync();

                ViewData["UomConversionId"] = id;
            }
            catch (Exception ex)
            {

                //throw ex;
            }
        }
    }
}

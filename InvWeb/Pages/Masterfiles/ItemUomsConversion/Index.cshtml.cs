using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Masterfiles.ItemUomsConversion
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ConversionViewModel> InvUomConversion { get;set; }

        public async Task OnGetAsync()
        {
            try
            {

                var tmpInvUomConversion = await _context.InvUomConversions.ToListAsync();

                if (tmpInvUomConversion != null)
                {
                    InvUomConversion = new List<ConversionViewModel>();
                }

                foreach (var conversion in tmpInvUomConversion)
                {
                    InvUomConversion.Add(new ConversionViewModel {
                        Id = conversion.Id,
                        UomsConversion = conversion,
                        UomFrom = GetUomDescById(conversion.InvUomId_base),
                        UomTo = GetUomDescById(conversion.InvUomId_into)
                    });
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public string GetUomDescById(int id)
        {
            try
            {
                return _context.InvUoms.Find(id).uom;
            }
            catch
            {

                return "NA";
            }
        }
    }

    public class ConversionViewModel 
    {
        public int Id { get; set; }
        public InvUomConversion UomsConversion { get; set; }
        public string UomFrom { get; set; }
        public string UomTo { get; set; }
    }
}

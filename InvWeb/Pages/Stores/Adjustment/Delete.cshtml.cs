using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using CoreLib.Inventory.Interfaces;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.DTO.Common.TrxHeader;

namespace InvWeb.Pages.Stores.Adjustment
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteModel> _logger;
        private readonly IItemTrxServices itemTrxServices;


        public DeleteModel(ILogger<DeleteModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }


        [BindProperty]
        public TrxHeaderDeleteModel TrxDeleteModel { get; set; }

        //public InvTrxHdr InvTrxHdr { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //InvTrxHdr = await _context.InvTrxHdrs
            //    .Include(i => i.InvStore)
            //    .Include(i => i.InvTrxHdrStatu)
            //    .Include(i => i.InvTrxType).FirstOrDefaultAsync(m => m.Id == id);
            TrxDeleteModel = await itemTrxServices.GetTrxHeaderDeleteModel_OnDeleteOnGet((int)id);

            if (TrxDeleteModel.InvTrxHdr == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrxDeleteModel.InvTrxHdr = await _context.InvTrxHdrs.FindAsync(id);

            if (TrxDeleteModel.InvTrxHdr != null)
            {
                //remove transactions detail items
                var itemList = await _context.InvTrxDtls.Where(i => i.InvTrxHdrId == TrxDeleteModel.InvTrxHdr.Id).ToListAsync();
                _context.InvTrxDtls.RemoveRange(itemList);

                _context.InvTrxHdrs.Remove(TrxDeleteModel.InvTrxHdr);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { storeId = TrxDeleteModel.InvTrxHdr.InvStoreId });
        }
    }
}

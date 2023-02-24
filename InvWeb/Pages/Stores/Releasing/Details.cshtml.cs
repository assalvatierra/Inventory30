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
using CoreLib.DTO.Releasing;

namespace InvWeb.Pages.Stores.Releasing
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public DetailsModel(ILogger<DetailsModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            ReleasingDetailsModel = new ReleasingDetailsModel();

        }

        //public InvTrxHdr InvTrxHdr { get; set; }
        public ReleasingDetailsModel ReleasingDetailsModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReleasingDetailsModel.InvTrxHdr = await itemTrxServices.GetInvTrxHdrsById((int)id)
                                             .FirstOrDefaultAsync();

            if (ReleasingDetailsModel.InvTrxHdr == null)
            {
                return NotFound();
            }

            ReleasingDetailsModel.InvTrxDtls = itemTrxServices.GetInvTrxDtlsById((int)id);

            return Page();
        }
    }
}

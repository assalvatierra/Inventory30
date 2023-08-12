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
using CoreLib.Interfaces;
using Inventory;

namespace InvWeb.Pages.Stores.Receiving
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IInvApprovalServices invApprovalServices;

        public DetailsModel(ILogger<DetailsModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            invApprovalServices = new InvApprovalServices(_context, _logger);
            ReceivingDetailsModel = new ReceivingDetailsModel();

        }

        //public InvTrxHdr InvTrxHdr { get; set; }
        public ReceivingDetailsModel ReceivingDetailsModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            ReceivingDetailsModel.InvTrxHdr = await itemTrxServices.GetInvTrxHdrsById((int)id)
                                         .FirstOrDefaultAsync();

            if (ReceivingDetailsModel.InvTrxHdr == null)
            {
                return NotFound();
            }

            ReceivingDetailsModel.InvTrxDtls = itemTrxServices.GetInvTrxDtlsById((int)id);
            
            //ReceivingDetailsModel.InvTrxApproval = invApprovalServices.GetExistingApproval((int)id);

            return Page();
        }
    }
}

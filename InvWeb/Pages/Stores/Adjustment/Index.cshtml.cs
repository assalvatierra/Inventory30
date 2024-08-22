using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using CoreLib.Inventory.Interfaces;
using CoreLib.DTO.Receiving;
using CoreLib.DTO.Common.TrxHeader;
using Microsoft.AspNetCore.Authorization;
using Inventory.DBAccess;

namespace InvWeb.Pages.Stores.Adjustment
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemTrxServices itemTrxServices;
        private readonly ILogger<IndexModel> _logger;
        private readonly DBMasterService dBMaster;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            dBMaster = new DBMasterService(_context, _logger);
        }

        public IList<InvTrxHdr> InvTrxHdrs { get;set; }
        public TrxHeaderIndexModel AdjustmentIndexModel { get; set; }

        [BindProperty]
        public string Status { get; set; }   // filter Parameter
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        private readonly int TYPE_ADJUSTMENT = 3;

        public async Task<ActionResult> OnGetAsync(int? storeId, string status)
        {

            if (storeId == null || storeId == 0)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(status))
            {
                status = "PENDING";
            }

            //AdjustmentIndexModel = await itemTrxServices.GetTrxHeaderIndexModel_OnGetAsync(InvTrxHdr, (int)storeId, TYPE_ADJUSTMENT, status, IsUserAdmin());

            AdjustmentIndexModel = new TrxHeaderIndexModel();

            InvTrxHdrs = await dBMaster.InvTrxHdrDb.GetInvTrxHdrsByTypeIdAndStoreId(TYPE_ADJUSTMENT, (int)storeId);

            InvTrxHdrs = itemTrxServices.FilterByStatus(InvTrxHdrs, status);

            AdjustmentIndexModel.InvTrxHdrs = InvTrxHdrs;
            AdjustmentIndexModel.StoreId = (int)storeId;
            AdjustmentIndexModel.Status = status;
            AdjustmentIndexModel.IsAdmin = IsUserAdmin();


            var trxCount = await _context.InvTrxHdrs
                .Where(i => i.InvTrxTypeId == TYPE_ADJUSTMENT && i.InvStoreId == storeId)
                .ToListAsync();

            ViewData["StatusCountRequest"] = itemTrxServices.FilterByStatus(trxCount, "PENDING").Count();
            ViewData["StatusCountApproved"] = itemTrxServices.FilterByStatus(trxCount, "APPROVED").Count();
            ViewData["StatusCountClosed"] = itemTrxServices.FilterByStatus(trxCount, "CLOSED").Count();


            ViewData["StoreId"] = storeId;
            ViewData["IsProcurementHead"] = IsUserProcHead();
            ViewData["IsAdmin"] = IsUserAdmin();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(status))
            {
                status = "PENDING";
            }

            AdjustmentIndexModel = new TrxHeaderIndexModel();

            InvTrxHdrs = await dBMaster.InvTrxHdrDb.GetInvTrxHdrsByTypeIdAndStoreId(TYPE_ADJUSTMENT, (int)storeId);

            InvTrxHdrs = itemTrxServices.FilterByStatus(InvTrxHdrs, status);

            AdjustmentIndexModel.InvTrxHdrs = InvTrxHdrs;
            AdjustmentIndexModel.StoreId = (int)storeId;
            AdjustmentIndexModel.Status = status;
            AdjustmentIndexModel.IsAdmin = IsUserAdmin();

            var trxCount = await _context.InvTrxHdrs
                .Where(i => i.InvTrxTypeId == TYPE_ADJUSTMENT && i.InvStoreId == storeId)
                .ToListAsync();

            ViewData["StatusCountRequest"] = itemTrxServices.FilterByStatus(trxCount, "PENDING").Count();
            ViewData["StatusCountApproved"] = itemTrxServices.FilterByStatus(trxCount, "APPROVED").Count();
            ViewData["StatusCountClosed"] = itemTrxServices.FilterByStatus(trxCount, "CLOSED").Count();

            ViewData["StoreId"] = storeId;
            ViewData["IsAdmin"] = IsUserAdmin();
            ViewData["IsProcurementHead"] = IsUserProcHead();
            return Page();
        }

        private bool IsUserAdmin()
        {
            return User.IsInRole("Admin");
        }
        private bool IsUserProcHead()
        {
            return User.IsInRole("Procurement-head");
        }

        public bool IsTransactionHaveApprovedRecord(int trxHdrId, string recordType)
        {
            if (_context.InvTrxApprovals.Where(t => t.InvTrxHdrId == trxHdrId).Count() > 0)
            {
                var trxApprovalRecord = _context.InvTrxApprovals.Where(t => t.InvTrxHdrId == trxHdrId).First();

                if (recordType == "Approved")
                {
                    if (!String.IsNullOrEmpty(trxApprovalRecord.ApprovedBy))
                    {
                        return true;
                    }
                }

                if (recordType == "Verified")
                {
                    if (!String.IsNullOrEmpty(trxApprovalRecord.VerifiedBy))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

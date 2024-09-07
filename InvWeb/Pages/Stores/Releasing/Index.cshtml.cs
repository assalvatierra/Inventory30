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
using Modules.Inventory;
using Microsoft.Extensions.Logging;
using CoreLib.DTO.Releasing;
using Microsoft.AspNetCore.Authorization;

namespace InvWeb.Pages.Stores.Releasing
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IItemTrxServices itemTrxServices;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
        }

        public IList<InvTrxHdr> InvTrxHdr { get;set; }
        public ReleasingIndexModel ReleasingIndexModel { get; set; }

        private readonly int TYPE_RELEASING = 2;
        private int STOREID;

        public async Task OnGetAsync(int? storeId, string status)
        {
            STOREID = (int)storeId;
            if (storeId == null)
            {
                //return NotFound();
            }

            if (!string.IsNullOrEmpty(status))
            {
                Status = status;
            }

            ReleasingIndexModel = new ReleasingIndexModel();

            InvTrxHdr = itemTrxServices.GetInvTrxHdrsByStoreId((int)storeId, TYPE_RELEASING).OrderByDescending(i => i.DtTrx).ToList();
            InvTrxHdr = itemTrxServices.FilterByStatus(InvTrxHdr, status);

            ReleasingIndexModel.InvTrxHdrs = this.InvTrxHdr;
            ReleasingIndexModel.Status = status;
            ReleasingIndexModel.StoreId = (int)storeId;
            ReleasingIndexModel.IsAdmin = IsUserRoleAdmin();

            var trxCount = await GetReleasingStatusCount();

            ViewData["StatusCountRequest"] = itemTrxServices.FilterByStatus(trxCount, "PENDING").Count();
            ViewData["StatusCountApproved"] = itemTrxServices.FilterByStatus(trxCount, "APPROVED").Count();

            ViewData["StoreId"] = storeId;
            ViewData["IsProcurementHead"] = User.IsInRole("Procurement-head");
            ViewData["IsAccounting"] = User.IsInRole("Accounting");

           
        }


        [BindProperty]
        public string Status { get; set; }   // filter Parameter
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        public async Task<IActionResult> OnPostAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                return NotFound();
            }


            ReleasingIndexModel = new ReleasingIndexModel();

            InvTrxHdr = itemTrxServices.GetInvTrxHdrsByStoreId((int)storeId, TYPE_RELEASING).ToList();
            InvTrxHdr = itemTrxServices.FilterByStatus(InvTrxHdr, status);

            ReleasingIndexModel.InvTrxHdrs = this.InvTrxHdr;
            ReleasingIndexModel.Status = status;
            ReleasingIndexModel.StoreId = (int)storeId;
            ReleasingIndexModel.IsAdmin = IsUserRoleAdmin();

            var trxCount = await GetReleasingStatusCount();

            ViewData["StatusCountRequest"] = itemTrxServices.FilterByStatus(trxCount, "PENDING").Count();
            ViewData["StatusCountApproved"] = itemTrxServices.FilterByStatus(trxCount, "APPROVED").Count();

            ViewData["StoreId"] = storeId;
            ViewData["IsProcurementHead"] = User.IsInRole("Procurement-head");
            ViewData["IsAccounting"] = User.IsInRole("Accounting");

            return Page();
        }

        private bool IsUserRoleAdmin()
        {
            return User.IsInRole("Admin");
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


                if (recordType == "Accounting")
                {
                    if (!String.IsNullOrEmpty(trxApprovalRecord.ApprovedAccBy))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

          
        public async Task<List<InvTrxHdr>> GetReleasingStatusCount()
        {
            return await _context.InvTrxHdrs
                .Where(i => i.InvTrxTypeId == TYPE_RELEASING && i.InvStoreId == STOREID)
                .ToListAsync();
        }
    }
}

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
using CoreLib.DTO.Receiving;
using CoreLib.Interfaces;
using Inventory;
using Microsoft.CodeAnalysis;

namespace InvWeb.Pages.Stores.Receiving
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemTrxServices itemTrxServices;
        private readonly IInvItemMasterServices invItemMasterServices;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            itemTrxServices = new ItemTrxServices(_context, _logger);
            invItemMasterServices = new InvItemMasterServices(_context, _logger);
        }

        public IList<InvTrxHdr> InvTrxHdr { get;set; }
        public ReceivingIndexModel ReceivingIndexModel { get; set; }

        private readonly int TYPE_RECEIVING = 1;

        public async Task<ActionResult> OnGetAsync(int? storeId, string status)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(status))
            {
                status = "ALL";
            }

            //ReceivingIndexModel = await itemTrxServices.GetReceivingIndexModel_OnIndexOnGetAsync(InvTrxHdr, (int)storeId, TYPE_RECEIVING, status, IsUserRoleAdmin());

            ReceivingIndexModel receivingIndexModel = new ReceivingIndexModel();

            InvTrxHdr = await _context.InvTrxHdrs
            .Include(i => i.InvStore)
            .Include(i => i.InvTrxHdrStatu)
            .Include(i => i.InvTrxType)
            .Include(i => i.InvTrxDtls)
                .ThenInclude(i => i.InvItem)
                .ThenInclude(i => i.InvUom)
            .Include(i => i.InvTrxDtls)
                .ThenInclude(i => i.InvItem)
                .ThenInclude(i => i.InvItemSpec_Steel)
                .ThenInclude(i => i.SteelMaterial)
            .Include(i => i.InvTrxDtls)
                .ThenInclude(i => i.InvItem)
                .ThenInclude(i => i.InvItemSpec_Steel)
                .ThenInclude(i => i.SteelMaterialGrade)
            .Where(i => i.InvTrxTypeId == TYPE_RECEIVING &&
                          i.InvStoreId == storeId)
            .ToListAsync();

            InvTrxHdr = itemTrxServices.FilterByStatus(InvTrxHdr, status);

            receivingIndexModel.InvTrxHdrs = InvTrxHdr;
            receivingIndexModel.StoreId = (int)storeId;
            receivingIndexModel.Status = status;
            receivingIndexModel.IsAdmin = IsUserRoleAdmin();

            ReceivingIndexModel = receivingIndexModel;

            var trxCount = await _context.InvTrxHdrs
                .Where(i => i.InvTrxTypeId == TYPE_RECEIVING && i.InvStoreId == storeId)
                .ToListAsync();

            ViewData["StatusCountRequest"] = itemTrxServices.FilterByStatus(trxCount, "PENDING").Count();
            ViewData["StatusCountApproved"] = itemTrxServices.FilterByStatus(trxCount, "APPROVED").Count();
            ViewData["StatusCountClosed"] = itemTrxServices.FilterByStatus(trxCount, "CLOSED").Count();

            ViewData["IsProcurementHead"] = User.IsInRole("Procurement-head");
            ViewData["IsAccounting"] = User.IsInRole("Accounting");
            return Page();
        }

        [BindProperty]
        public string Status { get; set; }   //this is the key bit
        [BindProperty]
        public string Orderby { get; set; }   //this is the key bit

        public async Task<IActionResult> OnPostAsync(int? storeId)
        {
            if (storeId == null)
            {
                return NotFound();
            }

            ReceivingIndexModel = await itemTrxServices.GetReceivingIndexModel_OnIndexOnPostAsync(InvTrxHdr, (int)storeId, TYPE_RECEIVING, Status, Orderby, IsUserRoleAdmin());

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
                        //Link TrxDtls and InvItemMasters
                        LinkItemMasterInvDtlsByHeaderId(trxHdrId);
                        return true;
                    }
                }
            }

            return false;
        }

        public void LinkItemMasterInvDtlsByHeaderId(int id)
        {

            var DtlItems = _context.InvTrxDtls.Where(i=>i.InvTrxHdrId == id).ToList();

            if (DtlItems.Count()  > 0)
            {
                foreach (var dtls in DtlItems)
                {
                    var invItemMaster = GetInvItemMasterByInvItemId(dtls.InvItemId);
                    if (invItemMaster != null)
                    {
                        invItemMasterServices.CreateItemMasterInvDtlsLink(invItemMaster.Id, dtls.InvItemId);
                    }
                }

            }
        }

        public InvItemMaster GetInvItemMasterByInvItemId(int id)
        {
            if (_context.InvItemMasters.Where(i => i.InvItemId == id).Count() > 0)
            {
                return _context.InvItemMasters.Where(i => i.InvItemId == id).FirstOrDefault();
            }

            return null;
        }
        
    }
}

using CoreLib.Inventory.Interfaces;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using DevExpress.Data.ODataLinq.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Inventory;
using PageConfigShared.Interfaces;
using PageConfigShared.Model;
using ReportViewModel.InvStore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static InvWeb.Pages.Stores.Printables.RequestFormModel;

namespace InvWeb.Pages.Stores.Printables
{
    public class FinalRequestFormModel : PageModel
    {
        private readonly ILogger<RequestFormModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IStoreServices _storeSvc;

        public TrxHdr _trxHdr;

        public enum rptFormView
        {
            RECEIVING = 1,
            RELEASING = 2,
            ADJUSTMENT = 3,
            PURCHASE_REQUEST = 4

        }

        public FinalRequestFormModel(ILogger<RequestFormModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(context, logger);
        }

        public async Task<IActionResult> OnGetAsync(int? id, int? type)
        {
            _trxHdr = new TrxHdr();

            if (id == null)
            {
                return NotFound();
            }

            if (type == null)
            {
                type = 1;
            }

            var request = await GetTrxHeaderByIdAsync((int)id);

            this._trxHdr = InitializeTrxHeader(_trxHdr, request);
            this._trxHdr.Details = AddDataToTrxDetails(_trxHdr.Details, request);

            if (type == (int)rptFormView.PURCHASE_REQUEST)
            {
                var PRrequest = await GetPRHeaderByIdAsync((int)id);
                this._trxHdr = InitializePRHeader(_trxHdr, PRrequest);
                this._trxHdr.Details = AddDataToPRDetails(_trxHdr.Details, PRrequest);
            }

            ViewData["Company"]  = "Vsteel Metal Asia";
            ViewData["Branch"]   = request.InvStore.StoreName;
            ViewData["SubTitle"] = GetFormSubHeaderTitle((int)type);

            InvTrxApproval Approvals = GetTrxInvTrxApproval((int)id);

            ViewData["PreparedBy"] = request.UserId;
            ViewData["ApprovedBy"] = Approvals.ApprovedAccBy;
            ViewData["PerformedBy"] = Approvals.EncodedBy;
            ViewData["VerifiedBy"] = Approvals.VerifiedBy;
            return Page();

        }


        public InvTrxApproval GetTrxInvTrxApproval(int id)
        {
            InvTrxApproval _approvals = _context.InvTrxApprovals
                .FirstOrDefault(i => i.Id == id);

            if (_approvals == null)
            {
                _approvals = new InvTrxApproval
                {
                    EncodedBy = " ",
                    ApprovedAccBy = " ",
                    VerifiedBy = " ",
                    InvTrxHdrId = id
                };
            }

            return _approvals;
        }


        public string GetFormSubHeaderTitle(int typeId)
        {
            switch (typeId)
            {
                case (int)rptFormView.RECEIVING:
                    return "Final Receiving Form";
                case (int)rptFormView.RELEASING:
                    return "Final Releasing Form";
                case (int)rptFormView.ADJUSTMENT:
                    return "Final PO Form";
                default:
                    return "Final Form";
            }

        }


        public async Task<InvTrxHdr> GetTrxHeaderByIdAsync(int id)
        {
            return await _context.InvTrxHdrs.Include(i => i.InvTrxDtls)
                .ThenInclude(i => i.InvItem)
                .Include(i => i.InvTrxType)
                .Include(i => i.InvTrxDtls).ThenInclude(i => i.InvTrxDtlOperator)
                .Include(i => i.InvTrxDtls).ThenInclude(i => i.InvUom)
                .Include(i => i.InvStore)
                .FirstOrDefaultAsync(i => i.Id == id);
        }


        public async Task<InvPoHdr> GetPRHeaderByIdAsync(int id)
        {
            return await _context.InvPoHdrs.Include(i => i.InvPoItems)
                .ThenInclude(i => i.InvItem)
                .Include(i => i.InvSupplier)
                .Include(i => i.InvPoHdrStatu)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public TrxHdr InitializeTrxHeader(TrxHdr tempTrxHdr, InvTrxHdr requestHdr)
        {
            if (requestHdr != null)
            {
                tempTrxHdr.Type = "Final Release Form";
                tempTrxHdr.Date = requestHdr.DtTrx;
                tempTrxHdr.Id = requestHdr.Id;
                tempTrxHdr.Address = " ";
                tempTrxHdr.Details = new List<TrxDetail>();
                tempTrxHdr.Party = requestHdr.Party;
                tempTrxHdr.Remarks = requestHdr.Remarks;

                return tempTrxHdr;
            }

            return new TrxHdr();
        }


        public TrxHdr InitializePRHeader(TrxHdr tempTrxHdr, InvPoHdr requestHdr)
        {
            if (requestHdr != null)
            {
                tempTrxHdr.Type = "Purchase Request";
                tempTrxHdr.Date = requestHdr.DtPo;
                tempTrxHdr.Id = requestHdr.Id;
                tempTrxHdr.Address = " ";
                tempTrxHdr.Details = new List<TrxDetail>();
                tempTrxHdr.PaidTo = requestHdr.InvSupplier.Name;

                return tempTrxHdr;
            }

            return new TrxHdr();
        }

        public List<TrxDetail> AddDataToTrxDetails(IList<TrxDetail> tempTrxDetails, InvTrxHdr requestHdr)
        {
            if (tempTrxDetails == null)
            {
                tempTrxDetails = new List<TrxDetail>();
            }

            if (requestHdr.InvTrxDtls.Count == 0)
            {
                return (List<TrxDetail>)tempTrxDetails;
            }

            int ItemCount = 0;
            foreach (var item in requestHdr.InvTrxDtls)
            {
                ItemCount += 1;

                var trxDetails = new TrxDetail
                {
                    Id = item.InvItemId,
                    Description = "(" + item.InvItem.Code + ") " + item.InvItem.Description,
                    Remarks = item.InvItem.Remarks,
                    Amount = 0,
                    Qty = item.ItemQty,
                    Uom = item.InvUom.uom,
                    Count = ItemCount,
                    Operation = item.InvTrxDtlOperator.Description,
                    LotNo = item.LotNo
                   
                };

                if (requestHdr.InvTrxTypeId == 3)
                {
                    trxDetails.Remarks = OperationActionRemarks(item.InvTrxDtlOperatorId) + item.InvItem.Remarks;
                }

                tempTrxDetails.Add(trxDetails);
            }
            return (List<TrxDetail>)tempTrxDetails;
        }


        public List<TrxDetail> AddDataToPRDetails(IList<TrxDetail> tempTrxDetails, InvPoHdr requestHdr)
        {
            if (tempTrxDetails == null)
            {
                tempTrxDetails = new List<TrxDetail>();
            }

            if (requestHdr.InvPoItems.Count == 0)
            {
                return (List<TrxDetail>)tempTrxDetails;
            }

            int ItemCount = 0;
            foreach (var item in requestHdr.InvPoItems)
            {
                ItemCount += 1;

                var trxDetails = new TrxDetail
                {
                    Id = item.InvItemId,
                    Description = "(" + item.InvItem.Code + ") " + item.InvItem.Description,
                    Remarks = item.InvItem.Remarks,
                    Amount = 0,
                    Qty = int.Parse(item.ItemQty),
                    Count = ItemCount
                };


                tempTrxDetails.Add(trxDetails);
            }
            return (List<TrxDetail>)tempTrxDetails;
        }

        private string OperationActionRemarks(int operationId)
        {
            switch (operationId)
            {
                case 1:
                    return "( Add ) ";
                case 2:
                    return "( Deduct ) ";
                default:
                    return "( Add ) ";
            }
        }

        public int GetTotalItemsCount()
        {
            return this._trxHdr.Details.Count;
        }
    }
}

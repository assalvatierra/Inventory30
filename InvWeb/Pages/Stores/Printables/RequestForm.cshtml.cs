using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoreLib.Inventory.Models;
using System.Linq;
using ReportViewModel.InvStore;
using PageConfigShared.Interfaces;
using PageConfigShared.Model;
using CoreLib.Models.Inventory;

namespace InvWeb.Pages.Stores.Printables
{
    public class RequestFormModel : PageModel
    {
        private readonly ILogger<RequestFormModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly StoreServices _storeSvc;
        public IPageConfigServices _pageConfigServices;

        public TrxHdr _trxHdr;
        public string rptView = "~/Areas/InvStore/TrxPrintForm.cshtml";

        public enum rptFormView
        {
            RECEIVING = 1,
            RELEASING = 2,
            ADJUSTMENT = 3,
            PURCHASE_REQUEST = 4

        }

        public RequestFormModel(ILogger<RequestFormModel> logger, ApplicationDbContext context, IPageConfigServices pageConfigSservices)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(context, logger);
            _pageConfigServices = pageConfigSservices;
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


            PageConfigInfo pInfo = GetReportFormByTrxType((int)type);
            if (pInfo != null)
            {
                this.rptView = pInfo.ViewName;

                if(pInfo.genericConfigKeys.Contains("Branch")) pInfo.genericConfigKeys["Branch"] = _storeSvc.GetStoreName(request.InvStoreId);

                this._trxHdr.pageSetting = pInfo.genericConfigKeys;
            }


            return Page();
        }

        public PageConfigInfo GetReportFormByTrxType(int type)
        {
            switch (type)
            {
                case (int)rptFormView.RECEIVING:
                    return this._pageConfigServices.getPageConfig("rpt001"); //rpt001 for Receiving
                case (int)rptFormView.RELEASING:
                    return this._pageConfigServices.getPageConfig("rpt002"); //rpt001 for Releasing
                case (int)rptFormView.ADJUSTMENT:
                    return this._pageConfigServices.getPageConfig("rpt003"); //rpt003 for Adjustments
                case (int)rptFormView.PURCHASE_REQUEST:
                    return this._pageConfigServices.getPageConfig("rpt004"); //rpt004 for PO
                default:
                    return this._pageConfigServices.getPageConfig("rpt002"); //default
            }
        }

        public async Task<InvTrxHdr> GetTrxHeaderByIdAsync(int id)
        {
            return await _context.InvTrxHdrs.Include(i => i.InvTrxDtls)
                .ThenInclude(i => i.InvItem)
                .Include(i => i.InvTrxType)
                .Include(i => i.InvTrxDtls).ThenInclude(i=>i.InvTrxDtlOperator)
                .Include(i=>i.InvTrxDtls).ThenInclude(i=>i.InvUom)
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

        public TrxHdr InitializeTrxHeader(TrxHdr tempTrxHdr ,InvTrxHdr requestHdr)
        {
            if (requestHdr != null)
            {
                tempTrxHdr.Type = requestHdr.InvTrxType.Type;
                tempTrxHdr.Date = requestHdr.DtTrx;
                tempTrxHdr.Id   = requestHdr.Id;
                tempTrxHdr.Address = " ";
                tempTrxHdr.Details = new List<TrxDetail>();

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
                    Operation = item.InvTrxDtlOperator.Description
                };

                if (requestHdr.InvTrxTypeId == 3)
                {
                    trxDetails.Remarks = OperationActionRemarks(item.InvTrxDtlOperatorId) + item.InvItem.Remarks ;
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

    }

}

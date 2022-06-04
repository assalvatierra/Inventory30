using InvWeb.Data;
using InvWeb.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebDBSchema.Models;
using System.Linq;
using ReportViewModel.InvStore;
using PageConfiguration.Interfaces;
using PageConfiguration.Model;

namespace InvWeb.Pages.Stores.Printables
{
    public class RequestVoucherModel : PageModel
    {
        private readonly ILogger<RequestVoucherModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly StoreServices _storeSvc;
        public IPageConfigServices _pageConfigServices;

        public TrxHdr _trxHdr;
        public string rptView = "~/Areas/InvStore/TrxPrintForm.cshtml";

        public enum rptVoucherView
        {
            RECEIVING = 1,
            RELEASING = 2,
            ADJUSTMENT = 3,
            PURCHASEORDER = 4

        }

        public RequestVoucherModel(ILogger<RequestVoucherModel> logger, ApplicationDbContext context, IPageConfigServices pageConfigSservices)
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


            PageConfigInfo pInfo = GetReportFormByTrxType((int)type);
            if (pInfo != null)
            {
                this.rptView = pInfo.ViewName;

                if(pInfo.genericConfigKeys.Contains("Branch")) pInfo.genericConfigKeys["Branch"] = "TEST";

                this._trxHdr.pageSetting = pInfo.genericConfigKeys;
                //this.processConfigKeys(pInfo.ConfigKeys);
            }

            //rptView = GetReportFormByTrxType((int)type);

            return Page();
        }

        public PageConfigInfo GetReportFormByTrxType(int type)
        {
            switch (type)
            {
                case (int)rptVoucherView.RECEIVING:
                    return this._pageConfigServices.getPageConfig("rpt001"); //rpt001 for Receiving
                case (int)rptVoucherView.RELEASING:
                    return this._pageConfigServices.getPageConfig("rpt002"); //rpt001 for Releasing
                case (int)rptVoucherView.ADJUSTMENT:
                    return this._pageConfigServices.getPageConfig("rpt003"); //rpt003 for Adjustments
                case (int)rptVoucherView.PURCHASEORDER:
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

        public TrxHdr InitializeTrxHeader(TrxHdr tempTrxHdr ,InvTrxHdr requestHdr)
        {
            if (requestHdr != null)
            {
                tempTrxHdr.Type = requestHdr.InvTrxType.Type;
                tempTrxHdr.Date = requestHdr.DtTrx;
                tempTrxHdr.Id   = requestHdr.Id;
                tempTrxHdr.Address = "NA";
                tempTrxHdr.Details = new List<TrxDetail>();

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

                tempTrxDetails.Add(new TrxDetail
                {
                    Id = item.InvItemId,
                    Description = "(" + item.InvItem.Code + ") " + item.InvItem.Description,
                    Remarks = item.InvItem.Remarks,
                    Amount = 0,
                    Qty = item.ItemQty,
                    Uom = item.InvUom.uom,
                    Count = ItemCount,
                    Operation = item.InvTrxDtlOperator.Description
                });
            }
            return (List<TrxDetail>)tempTrxDetails;
        }

    }

}

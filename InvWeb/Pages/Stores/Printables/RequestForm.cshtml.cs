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
            PURCHASEORDER = 4

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


            PageConfigInfo pInfo = GetReportFormByTrxType((int)type);
            if (pInfo != null)
            {
                this.rptView = pInfo.ViewName;
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
                case (int)rptFormView.RECEIVING:
                    return this._pageConfigServices.getPageConfig("rpt001"); //rpt001 for Receiving
                case (int)rptFormView.RELEASING:
                    return this._pageConfigServices.getPageConfig("rpt002"); //rpt001 for Releasing
                case (int)rptFormView.ADJUSTMENT:
                    return this._pageConfigServices.getPageConfig("rpt003"); //rpt003 for Adjustments
                case (int)rptFormView.PURCHASEORDER:
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
                .Include(i => i.InvTrxDtls)
                .ThenInclude(i=>i.InvTrxDtlOperator)
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
                    Count = ItemCount,
                    Operation = item.InvTrxDtlOperator.Description
                });
            }
            return (List<TrxDetail>)tempTrxDetails;
        }

    }

}

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

namespace InvWeb.Pages.Stores.Printables
{
    public class RequestVoucherModel : PageModel
    {
        private readonly ILogger<RequestVoucherModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly StoreServices _storeSvc;

        public VoucherForm voucher;

        public RequestVoucherModel(ILogger<RequestVoucherModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _storeSvc = new StoreServices(context, logger);

            voucher = new VoucherForm();
        }


        public async Task<IActionResult> OnGetAsync(int? id, int? type)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (type == null)
            {
                type = 1;
            }

            var request = await _context.InvTrxHdrs.Include(i => i.InvTrxDtls)
                .ThenInclude(i=>i.InvItem)
                .Include(i => i.InvTrxType)
                .FirstOrDefaultAsync(i=>i.Id == id);

            voucher.Type = request.InvTrxType.Type;
            voucher.Date = request.DtTrx;
            voucher.Id = (int)id;
            voucher.Address = "NA";

            voucher.Items = new List<VoucherItems>();
            int ItemCount = 0;
            int QtyCount = 0;
            foreach (var item in request.InvTrxDtls)
            {
                ItemCount += 1;
                QtyCount += item.ItemQty;

                voucher.Items.Add(new VoucherItems
                {
                    Id = item.InvItemId,
                    Description = "(" + item.InvItem.Code + ") " + item.InvItem.Description + " "+ item.InvItem.Remarks,
                    Amount = 0,
                    Qty = item.ItemQty,
                    Count = ItemCount 
                });
            }

            voucher.TotalCount = ItemCount;
            voucher.TotalQty = QtyCount;

            return Page();
        }

    }

    public class VoucherItems
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public int Qty { get; set; }
        public int Count { get; set; }

    }

    public class VoucherForm
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string PaidTo { get; set; } 
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public List<VoucherItems> Items { get; set; }

        public int TotalCount { get; set; }
        public int TotalQty { get;set; }
    }
}

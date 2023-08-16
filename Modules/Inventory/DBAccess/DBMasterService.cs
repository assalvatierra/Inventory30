using CoreLib.Interfaces.DBAccess;
using CoreLib.Inventory.Models;
using CoreLib.Models.Inventory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DBAccess
{
    public class DBMasterService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public IStoreDb StoreDb;
        public IInvTrxHdrStatusDb InvTrxHdrStatusDb;
        public IInvTrxHdrDb InvTrxHdrDb;
        public IInvTrxDtlOperatorDb InvTrxDtlOperatorDb;
        public IInvTrxDtlDb InvTrxDtlDb;
        public IInvPOHdrDb InvPOHdrDb;
        public IInvPOItemDb InvPOItemDb;
        public IInvSupplierDb InvSupplierDb;
        public IInvPOApprovalDb InvPOApprovalDb;
        public IInvTrxApprovalDb InvTrxApprovalDb;


        public DBMasterService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            StoreDb = new StoreDb(_context, _logger);
            InvTrxHdrStatusDb = new InvTrxHdrStatusDb(_context, _logger);
            InvTrxHdrDb = new InvTrxHdrDb(_context, _logger);
            InvTrxDtlOperatorDb = new InvTrxDtlOperatorDb(_context, _logger);
            InvTrxDtlDb = new InvTrxDtlDb(_context, _logger);
            InvPOHdrDb = new InvPOHdrDb(_context, _logger);
            InvPOItemDb = new InvPOItemDb(_context, _logger);
            InvSupplierDb = new InvSupplierDb(_context, _logger);
            InvPOApprovalDb = new InvPOApprovalDb(_context, _logger);
            InvTrxApprovalDb  = new InvTrxApprovalDb(_context, _logger);

        }

        public IStoreDb GetStoreDb() {  return StoreDb; }
        public IInvTrxHdrStatusDb GetInvTrxHdrStatusDb() { return InvTrxHdrStatusDb; }
        public IInvTrxHdrDb GetInvTrxHdrDb() { return InvTrxHdrDb; }
        public IInvTrxDtlOperatorDb GetInvTrxDtlOperatorDb() { return InvTrxDtlOperatorDb; }
        public IInvTrxDtlDb GetInvTrxDtlDb() { return InvTrxDtlDb; }
        public IInvPOHdrDb GetInvPOHdrDb() { return InvPOHdrDb; }
        public IInvPOItemDb GetInvPOItemDb() { return InvPOItemDb; }
        public IInvSupplierDb GetInvSupplierDb() { return InvSupplierDb; }
        public IInvPOApprovalDb GetInvPOApprovalDb() { return InvPOApprovalDb; }
        public IInvTrxApprovalDb GetInvTrxApprovalDb() { return InvTrxApprovalDb; }

    }
}

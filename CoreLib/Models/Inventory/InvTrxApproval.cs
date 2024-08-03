using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Models.Inventory
{
    public partial class InvTrxApproval
    {
        public int Id { get; set; }
        public string? ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string? VerifiedBy { get; set; }
        public Nullable<System.DateTime> VerifiedDate { get; set; }
        public string EncodedBy { get; set; }
        public System.DateTime EncodedDate { get; set; }
        public int InvTrxHdrId { get; set; }
        public string? ApprovedAccBy { get; set; }
        public Nullable<System.DateTime> ApprovedAccDate { get; set; }

        public virtual InvTrxHdr InvTrxHdr { get; set; }
    }
}

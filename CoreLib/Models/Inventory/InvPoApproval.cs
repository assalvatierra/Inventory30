using CoreLib.Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Models.Inventory
{
    public partial class InvPOApproval
    {
        public int Id { get; set; }
        public string ApprovedBy { get; set; }
        public System.DateTime ApprovedDate { get; set; }
        public string VerifiedBy { get; set; }
        public System.DateTime VerifiedDate { get; set; }
        public string EncodedBy { get; set; }
        public System.DateTime EncodedDate { get; set; }
        public int InvPoHdrId { get; set; }

        public virtual InvPoHdr InvPoHdr { get; set; }
    }
}
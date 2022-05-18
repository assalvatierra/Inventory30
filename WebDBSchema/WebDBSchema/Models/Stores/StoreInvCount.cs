using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDBSchema.Models.Stores
{
    public class StoreInvCount
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Available { get; set; }
        public int OnHand { get; set; }
        public int ReceivePending { get; set; }
        public int ReceiveAccepted { get; set; }
        public int ReleaseRequest { get; set; }
        public int ReleaseReleased { get; set; }
        public int Adjustments { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }

        public ICollection<InvWarningLevel> InvWarningLevels { get; set; }
    }

}

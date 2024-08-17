using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Receiving
{
    public class ReceivingTrxItemApiModel
    {
            public int Id { get; set; }
            public int ItemId { get; set; }
            public string LotNo { get; set; }
            public string BatchNo { get; set; }
            public int BrandId { get; set; }
            public int OriginId { get; set; }
            public int Qty { get; set; }
            public int UomId { get; set; }
            public int AreaId { get; set; }
            public string Remarks { get; set; }


    }


    public class ReleasingTrxItemApiModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string LotNo { get; set; }
        public string BatchNo { get; set; }
        public int BrandId { get; set; }
        public int OriginId { get; set; }
        public int Qty { get; set; }
        public int UomId { get; set; }
        public int AreaId { get; set; }
        public string Remarks { get; set; }
        public int TrxId { get; set; }


    }
}

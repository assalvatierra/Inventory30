using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Adjustment
{
    public class AddItemReleasingDTO
    {
        public int InvItemId;
        public int HdrId;
        public int UomId;
        public int Qty;
        public int OperatorId;
        public int BrandId;
        public int OriginId;
        public string? LotNo;
        public string? BatchNo;
        public string? Remarks;
        public int AreaId;

    }

    public class EditItemReleasingDTO
    {
        public int InvItemId;
        public int InvTrxDetailsId;
        public int UomId;
        public int Qty;
        public int OperatorId;
        public int BrandId;
        public int OriginId;
        public string? LotNo;
        public string? BatchNo;
        public string? Remarks;
        public int AreaId;

    }
}

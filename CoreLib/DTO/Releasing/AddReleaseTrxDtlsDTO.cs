using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DTO.Releasing
{
    public class AddReleaseTrxDtlsDTO
    {
        public int hdrId { get; set; }
        public int invId { get; set; }
        public int qty { get; set; }
        public int uomId { get; set; }
        public int lotNo { get; set; }
        public string batchNo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class BillReasonDTO
    {
        public int BillReasonId { get; set; }
        public string? BillReasonCode { get; set; }
        public string? BillTypeCode { get; set; }
        public string? BillReasonDesc { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

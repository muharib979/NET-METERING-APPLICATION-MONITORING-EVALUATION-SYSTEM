using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MISCBILL
{
    public class BillReason
    {
        public int? BILL_REASON_ID { get; set; }
        public string? BILL_REASON_CODE { get; set; }
        public string? BILL_TYPE_CODE { get; set; }
        public string? BillTypeCode { get; set; }
        public string? BILL_REASON_DESC { get; set; }
        public string? REMARKS { get; set; }
        public string? STATUS { get; set; }
        public string? CREATE_BY { get; set; }
        public string? UPDATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
}

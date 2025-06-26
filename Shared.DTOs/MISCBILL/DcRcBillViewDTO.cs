using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class DcRcBillViewDTO
    {
        public string? BillNumber { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? NidNumber { get; set; }
        public string? Tariff { get; set; }
        public decimal? BillAmount { get; set; }
        public int? Paid { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DcDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? RcDate { get; set; }
        public string? LocationCode { get; set; }
    }
}

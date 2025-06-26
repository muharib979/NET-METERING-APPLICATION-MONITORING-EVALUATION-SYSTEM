using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class CustomerBillDetailsDTO
    {
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? LocationCode { get; set; }
        public string? AreaCode { get; set; }
        public string? BillNumber { get; set; }
        public string? BillTypeCode { get; set; }
        public string? NidNumber { get; set; }
        public string? Tariff { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? DueDate { get; set; }
        public string? PaymentDate { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public string? Paid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ConsumerBill
{
    public class ConsumerBillDTO
    {
        public string? BillNumber { get; set; }
        public string? CustomerNumber { get; set; }
        public string? LocationCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? Phone { get; set; }
        public string? Paid { get; set; }
    }
}

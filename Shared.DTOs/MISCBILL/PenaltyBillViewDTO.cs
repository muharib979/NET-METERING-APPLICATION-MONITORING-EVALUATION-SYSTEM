using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PenaltyBillViewDTO
    {
        public string? BillNumber { get; set; }
        public string? MeterType { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public string? CustomerNumber { get; set; }
        public int? InstallmentNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? Tariff { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? UnitUse { get; set; }
        public string? NidNumber { get; set; }
        public string? Address { get; set; }
        public string? Location { get; set; }
        public int? Paid { get; set; }
        public DateTime? DuaDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? LocationCode { get; set; }
    }
}

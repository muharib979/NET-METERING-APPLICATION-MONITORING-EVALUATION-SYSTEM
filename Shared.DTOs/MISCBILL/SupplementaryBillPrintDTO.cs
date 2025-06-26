using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class SupplementaryBillPrintDTO
    {
        public string? CustomerName { get; set; }
        public string? FatherName { get; set; }
        public string? LocationName { get; set; }
        public string? CustomerAddress { get; set; }
        public string? LocationAddress { get; set; }
        public string? MonthName { get; set; } //
        public string? BillNumber { get; set; }
        public string? BillCheckDigit { get; set; }
        public string? IssueDate { get; set; }
        public string? LocationCode { get; set; }
        public string? WalkOrd { get; set; }
        public string? BillGroup { get; set; }
        public string? BookNo { get; set; }
        public string? PrvAcc { get; set; }
        public string? CustomerNumber { get; set; }
        public string? LastPaymentDate { get; set; }
        public string? Tariff { get; set; }
        public string? BsCode { get; set; }
        public string? MeterNumber { get; set; }
        public string? MeterType { get; set; }
        public string? MeterCondition { get; set; }
        public string? MobileNumber { get; set; }
        public string? NidNumber { get; set; }
        public string? ImposedBy { get; set; }
        public string? BillReason { get; set; }
        public int? PenaltyUnit { get; set; }
        public int? InstallmentNo { get; set; }
        public Decimal? PrincipleAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalInstallmentTaka { get; set; }
        public string? InstallmentDueDate { get; set; }
    }
}

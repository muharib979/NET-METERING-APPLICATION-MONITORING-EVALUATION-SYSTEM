using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PenaltyBillPrintDTO
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
        public decimal? PenaltyUnit { get; set; } 
        public decimal? SrUnit { get; set; } 
        public decimal? PeakUnit { get; set; } 
        public decimal? OffPeakUnit { get; set; } 
        public int? InstallmentNo { get; set; } 
        public decimal? PrincipleAmount { get; set; } 
        public decimal? PeakAmount { get; set; } 
        public decimal? OffPeakAmount { get; set; } 
        public decimal? VatAmount { get; set; } 
        public decimal? TotalAmount { get; set; } 
        public decimal? TotalInstallmentTaka { get; set; } 
        public string? InstallmentDueDate { get; set; } 
        public string? DcTypeCode { get; set; } 
        public string? AreaCode { get; set; } 
        public string? Remarks { get; set; }
        public string? Paid { get; set; }
        public decimal LpsAmountAfterDue { get; set; }
        public decimal TotalAmountAfterDue { get; set; }

    }
}

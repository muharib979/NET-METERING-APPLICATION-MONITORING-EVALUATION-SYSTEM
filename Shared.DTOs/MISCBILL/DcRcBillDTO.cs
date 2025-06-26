using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class DcRcBillDTO
    {
        public string? AreaCode { get; set; }
        public string? BillReasonCode { get; set; }
        public string? BusinessType { get; set; }
        public int? CustId { get; set; }
        public string? CustomerAddr { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerNumber { get; set; }
        public string? BillNumber { get; set; }
        public string? DueDate { get; set; }
        public string? ImposedByCode { get; set; }
        public decimal? LastReading { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? MeterCondition { get; set; }
        public string? MeterNum { get; set; }
        public string? MeterTypeDesc { get; set; }
        public string? MeterTypeCode { get; set; }
        public string MobileNumber { get; set; }
        public string? MultypliBy { get; set; }
        public string NidNumber { get; set; }
        public decimal? PenaltyUnit { get; set; }
        public decimal? LastReadingPeak { get; set; }
        public decimal? LastReadingOffPeak { get; set; }
        public decimal? OffPeakUnit { get; set; }
        public decimal? PeakUnit { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public string? PrvAcNo { get; set; }
        public string? TariffDesc { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DcAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public string? p_REMARK { get; set; }
        public string? UserName { get; set; }
        public string? MeterConditionCode { get; set; }
        public string? MeterConditionDesc { get; set; }
        public string? BusinessTypeCode { get; set; }
        public decimal? SrAmount { get; set; }
        public decimal? PeakAmount { get; set; }
        public decimal? OffPeakAmount { get; set; }
        public string? DcTypeCode { get; set; }
        public decimal? DcTypeDesc { get; set; }
        public string LastReadingDate { get; set; }
        public string DcDate { get; set; }
        public List<FileSaveDto>? FileList { get; set; }
    }
}

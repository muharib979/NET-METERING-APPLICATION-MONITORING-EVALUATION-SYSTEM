using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PenaltyBillPrepaidDTO
    {
        public int CustId { get; set; }
        public string? CustomerName { get; set; }
        public string? BillNumber { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerAddr { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? AreaCode { get; set; }
        public string? BusinessType { get; set; }
        public string? TariffDesc { get; set; }
        public string? MeterNum { get; set; }
        public string? MeterTypeDesc { get; set; }
        public string? PrvAcNo { get; set; }
        public string? MeterCondition { get; set; }
        public string? MeterTypeCode { get; set; }
        public string? MeterConditionCode { get; set; }
        public string? MeterConditionDesc { get; set; }
        public string? BusinessTypeCode { get; set; }
        public string? NidNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? BillReasonCode { get; set; }
        public string? ImposedByCode { get; set; }
        public string? BookNumber { get; set; }
        public string? MeterType { get; set; }
        public string? FatherName { get; set; }
        public decimal? LastReading { get; set; }
        public decimal? LastReadingPeak { get; set; }
        public decimal? LastReadingOffPeak { get; set; }
        public decimal? PeakAmount { get; set; }
        public decimal? OffPeakAmount { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? LastBillReadingDate { get; set; }
        public string? ReadingDate { get; set; }

        public int? SanctionedLoad { get; set; }
        public int? ConnectionLoad { get; set; }
        public string? WlkOrd { get; set; }
        public string? BookNo { get; set; }
        public string? BillGroup{ get; set; }
        public string? BillCycleCode { get; set; }
        public string p_USER { get; set; }
        public List<FileSaveDto>? FileList { get; set; }

    }
}

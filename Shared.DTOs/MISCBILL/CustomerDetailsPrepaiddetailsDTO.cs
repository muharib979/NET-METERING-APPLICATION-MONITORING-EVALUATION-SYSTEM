using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class CustomerDetailsPenaltyBillPrepaidDTO
    {
        
            public int CustId { get; set; }
            public string? CustomerName { get; set; }
            public string? FatherName { get; set; }
            public string? BillNumber { get; set; }
            public string? CustomerNumber { get; set; }
            public string? LocationCode { get; set; }
            public string? LocationName { get; set; }
            public string? LocationDeptCode { get; set; }
            public string? AreaCode { get; set; }
            public int? TariffId { get; set; }
            public string? BusinessType { get; set; }
            public string? BusinessTypeCode { get; set; }
            public string? TariffDesc { get; set; }
            public string? MeterNum { get; set; }
            public string? MeterTypeDesc { get; set; }
            public string? PrvAcNo { get; set; }
            public string? CustomerAddr { get; set; }
            public string? MeterCondition { get; set; }
            public string? MeterTypeCode { get; set; }
            public string? MeterConditionCode { get; set; }
            public string? MeterConditionDesc { get; set; }
            public string? NidNumber { get; set; }
            public string? MobileNumber { get; set; }
            public string? BillReasonCode { get; set; }
            public string? MultypliBy { get; set; }
            public string? DcTypeCode { get; set; }
            public string? ImposedByCode { get; set; }
            public string? BookNumber { get; set; }
            public string? BillGroup { get; set; }
            public string? MeterType { get; set; }
            public decimal? LastReading { get; set; }
            public decimal? PenaltyUnit { get; set; }
            public decimal? PrincipleAmount { get; set; }
            public decimal? VatAmount { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? PeakUnit { get; set; }
            public decimal? DcAmount { get; set; }
            public decimal? OffPeakUnit { get; set; }
            public decimal? LastBillReadingSr { get; set; }
            public decimal? LastBillReadingPk { get; set; }
            public decimal? LastBillReadingOfPk { get; set; }
            public decimal? LastReadingPeak { get; set; }
            public decimal? LastReadingOffPeak { get; set; }
            public decimal? PeakAmount { get; set; }
            public decimal? OffPeakAmount { get; set; }
            public decimal? SupplymentaryAmount { get; set; }
            public DateTime? DueDate { get; set; }
            public DateTime? LastReadingDate { get; set; }
            public DateTime? LastBillReadingDate { get; set; }
            public DateTime? ReadingDate { get; set; }
            public DateTime? DcDate { get; set; }

            public int? SanctionedLoad { get; set; }
            public int? ConnectionLoad { get; set; }
            public string WlkOrd { get; set; }
            public string BookNo { get; set; }
            public string Tariff { get; set; }
            public string BillCycleCode { get; set; }

        public int? Xformer_day_rent { get; set; }
        public decimal? ArrearAmount { get; set; }

        public List<FileDto>? FileList { get; set; }

    }


}

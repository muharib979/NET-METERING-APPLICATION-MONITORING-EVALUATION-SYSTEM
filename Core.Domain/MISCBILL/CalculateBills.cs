using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MISCBILL
{
    public class CalculateBills
    {
        public decimal? v_Tot_prn_Amt { get; set; }
        public decimal? PRN_AMOUNT { get; set; }
        public decimal? VAT_AMOUNT { get; set; }
        public decimal? SR_Amount { get; set; }
        public decimal? Peak_Amount { get; set; }
        public decimal? OffPeak_Amount { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public string? CustomerNumber { get; set; }
        public decimal? PenaltyUnit { get; set; }
        public decimal? OffPeakUnit { get; set; }
        public decimal? PeakUnit { get; set; }
        public string? TariffDesc { get; set; }
        public string? LocationCode { get; set; }
        public string? BillReasonCode { get; set; }
        public string? MultypliBy { get; set; }
        public string? P_USER { get; set; }
        public string? MeterTypeCode { get; set; }
        public decimal? DcAmount { get; set; }
        public decimal? SupplymentaryAmount { get; set; }
        public int? xformer_kva { get; set; }
        public int? xformer_day_rent { get; set; }
    }
}

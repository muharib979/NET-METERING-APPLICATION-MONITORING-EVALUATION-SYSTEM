using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MISCBILL
{
    public class CustomerDetails
    {
        public int CUST_ID { get; set; }
        public string? CUSTOMER_NAME { get; set; }
        public string? BillNumber { get; set; }
        public string? CustomerNumber { get; set; }
        public string? Father_Name { get; set; }
        public string? LOCATION_CODE { get; set; }
        public string? LOCATION_NAME { get; set; }
        public string? AREA_CODE { get; set; }
        public int? TARIFF_ID { get; set; }
        public string? TARIFF { get; set; }
        public string? TARIFF_DESC { get; set; }
        public string? BUSINESS_TYPE { get; set; }
        public string? METER_NUM { get; set; }
        public string? METER_TYPE_DESC { get; set; }
        public string? PRV_AC_NO { get; set; }
        public string? CUSTOMER_ADDR { get; set; }
        public string? METER_COND { get; set; }
        public string? METER_TYPE_Code { get; set; }
        public string? METER_CODINTION_CODE { get; set; }
        public string? METER_CODINTION_DESC { get; set; }
        public string? BUSINESS_TYPE_CODE { get; set; }
        public string? NidNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? BillReasonCode { get; set; }
        public string? MultypliBy { get; set; }
        public string? ImposedByCode { get; set; }
        public string? DcTypeCode { get; set; }
        public decimal? LastReading { get; set; }
        public decimal? PenaltyUnit { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public decimal? DcAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PeakUnit { get; set; }
        public decimal? SupplymentaryAmount { get; set; }
        public decimal? OffPeakUnit { get; set; }
        public decimal? ArrearAmount{ get; set; }
        public decimal? LastReadingPeak { get; set; }
        public decimal? LastReadingOffPeak { get; set; }
        public decimal? PeakAmount { get; set; }
        public decimal? OffPeakAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DcDate { get; set; }
        public DateTime? LAST_READING_DATE { get; set; }
        public int? XFORMER_KVA { get; set; }
        public int? XFORMER_DAY_RENT { get; set; }
        public string? DEPTCODE { get; set; }
        public string? BillGroup { get; set; }
        public string? BookNumber { get; set; }
        public int? SanctionedLoad { get; set; }
        public decimal? LastBillReadingSr { get; set; }
        public decimal? LastBillReadingPk { get; set; }
        public decimal? LastBillReadingOfPk { get; set; }
        public string WALKING_SEQUENCE { get; set; }
        public string Wlk_ord { get; set; }
        public string BILL_GROUP { get; set; }
        public string BOOK_NO { get; set; }
        public string bill_cycle_code { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.NonBengali
{
    public class NonBengaliDTOs
    {
        public string NonbengaliNameCode { get; set; }
        public string NameBn { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameBn{ get; set; }
        public int MeterCount { get; set; }
        public decimal Prn { get; set; }
        public decimal Lps { get; set; }
        public decimal Vat { get; set; }
        //public decimal PreviousArrear { get; set; }
        //public decimal CurrentMonthBill { get; set; }
        //public decimal TotalCollection { get; set; }
        //public decimal CurrentArrear { get; set; }

        public decimal TotalArrear { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public int CustomerNo { get; set; }
        public string IsStatic { get; set; }
    }

    public class NonBengaliSummaryDTO 
    {
        public string NonBengaliNameCode { get; set; }
        public string NonBengaliNameBn { get; set; }
        public string LocationCode { get; set; }
        public string MeterCount { get; set; }
        public decimal Prn { get; set; }
        public decimal Lps { get; set; }
        public decimal TotalArrear { get; set; }
        public string LocationName { get; set; }
        public int IsStatic { get; set; }
        public string BillCycleCode { get; set; }
    }

    public class OnlineNonBengaliSummaryDTO 
    {
        public string ConsumerNo { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameBn { get; set; }
        public int MeterCount { get; set; }
        public decimal Prn { get; set; }
        public decimal Lps { get; set; }
        public decimal Vat { get; set; }
        public decimal CurrPrin { get; set; }
        public decimal CurrLps { get; set; }
        public decimal CurrVat { get; set; }
        public decimal TotalAmount { get; set; }
        public string LocationDesc { get; set; }
        public string BillCycleCode { get; set; }
        public string NonBengaliCampCode { get; set; }
        public string NonBengaliNameBn { get; set; }
        public string NonbengaliName { get; set; }
        public string ZoneName { get; set; }
    }

    public class OnlineNonBengaliSummaryMergeDTO
    {
        public string ConsumerNo { get; set; }
        public string Address { get; set; }
        public string MeterCount { get; set; }
        public string NonbengaliName { get; set; }
        public string CustomerName { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameBn { get; set; }
        public string LocationDesc { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public decimal ArrearReceiptAmount { get; set; }
        public decimal CurrReceiptVat { get; set; }
        public decimal CurrReceiptAmt { get; set; }
        public decimal TotalArrearAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public double TotalReceiptAmount { get; set; }
        public decimal CurrReceiptPrincipal { get; set; }
        public decimal TotalPrincipalArrear { get; set; }
        public decimal TotalLpsArrear { get; set; }
        public decimal TotalVatArrear { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearVat { get; set; }
        public decimal CurrPrin { get; set; }
        public decimal CurrPrincipal { get; set; }
        public decimal CurrLps { get; set; }
        public decimal CurrVat { get; set; }
        public string NonBengaliNameBn { get; set; }
        public string NonBengaliCampCode { get; set; }
        public decimal TotalReceiptPrincipal { get; set; }
        public decimal TotalReceiptVat { get; set; }
        public string BillCycleCode { get; set; }
    }

    public class OnlineNonBengaliModel
    {
        public List<OnlineNonBengaliSummaryDTO>? NonbengliDetailsDynamicDataList { get; set; }
        public List<NonBengaliSummaryDTO>? NonbengliDetailsStaticDataList { get; set; }
        public List<OnlineNonBengaliSummaryDTO>? NonbengliDetailsDataList { get; set; }
    }
}

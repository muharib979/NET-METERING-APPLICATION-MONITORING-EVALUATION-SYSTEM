using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class MinistrySummaryDto: MinistrySummaryMinistryCountDto
    {
        public string DbName { get; set; }
        public string MinistryCode { get; set; }
        public string MinistryName { get; set; }
        public string MinistryNameBn { get; set; }
        public int Noc { get; set; }
        public decimal Lps { get; set; }
        public decimal Vat { get; set; }
        public decimal Prn { get; set; }
        public decimal Total { get; set; }
        public string DbCode { get; set; }
        public string ZoneCode { get; set; }
    }

    public class MinistrySummaryMinistryCountDto
    {
        public decimal ChittagongCount { get; set; }
        //public decimal ChittagongPrn { get; set; }
        public decimal ChittagongPrevArrearAmt { get; set; }
        public decimal ChittagongCurrMonthBill { get; set; }
        public decimal ChittagongCollectionAmt { get; set; }
        public decimal ChittagongTotalArrearAmt { get; set; }

        public decimal ComillaCount { get; set; }
        //public decimal ComillaPrn { get; set; }
        public decimal ComillaPrevArrearAmt { get; set; }
        public decimal ComillaCurrMonthBill { get; set; }
        public decimal ComillaCollectionAmt { get; set; }
        public decimal ComillaTotalArrearAmt { get; set; }

        public decimal SylhetCount { get; set; }
        //public decimal SylhetPrn { get; set; }
        public decimal SylhetPrevArrearAmt { get; set; }
        public decimal SylhetCurrMonthBill { get; set; }
        public decimal SylhetCollectionAmt { get; set; }
        public decimal SylhetTotalArrearAmt { get; set; }

        public decimal MymensinghCount { get; set; }
        //public decimal MymensinghPrn { get; set; }
        public decimal MymensinghPrevArrearAmt { get; set; }
        public decimal MymensinghCurrMonthBill { get; set; }
        public decimal MymensinghCollectionAmt { get; set; }
        public decimal MymensinghTotalArrearAmt { get; set; }


        public decimal KishoreganjCount { get; set; }
        public decimal KishoreganjPrn { get; set; }

        public decimal MoulvibazarCount { get; set; }
        public decimal MoulvibazarPrn { get; set; }

        public decimal TangailCount { get; set; }
        public decimal TangailPrn { get; set; }

        public decimal JamalpurCount { get; set; }
        public decimal JamalpurPrn { get; set; }
    }

    public class PoliceArrearDetailsDTO
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string LocationCode { get; set; }
        public string LocationDesc { get; set; }
        //public decimal Prn { get; set; }
        //public decimal Lps { get; set; }
        //public decimal Vat { get; set; }
        //public decimal Total { get; set; }
        public decimal PrevArrearAmt { get; set; }
        public decimal CurrMonthBill { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalArrearAmt { get; set; }
    }

    public class PoliceArrearSummaryDTO 
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        //public decimal Prn { get; set; }
        //public decimal Lps { get; set; }
        //public decimal Vat { get; set; }
        //public decimal Total { get; set; }
        public decimal PrevArrearAmt { get; set; }
        public decimal CurrMonthBill { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalArrearAmt { get; set; }
    }

}

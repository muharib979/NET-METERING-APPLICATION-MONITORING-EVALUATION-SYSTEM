using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Railway
{
    public class RailwayDTO
    {
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ZoneCode { get; set; }
        public string CicleCode { get; set; }
        public string ZoneName { get; set; }
         public string CircleName { get; set; }
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

    public class RailwaySummaryDTO 
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        //public decimal TotalArrearPrincipal { get; set; }
        //public decimal TotalArrearLps { get; set; }
        //public decimal TotalArrearVat { get; set; }
        //public decimal TotalArrearAmount { get; set; }
        public decimal PrevArrearAmt { get; set; }
        public decimal CurrMonthBill { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal TotalArrearAmt { get; set; }
        public string CicleCode { get; set; }
        public string CircleName { get; set; }
    }
}

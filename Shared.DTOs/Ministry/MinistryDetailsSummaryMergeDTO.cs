using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class MinistryDetailsSummaryMergeDTO
    {
        public int Custid { get; set; }
        public int ConsumerNo { get; set; }
        public string LocationCode { get; set; }
        public string MinistryCode { get; set; }
        public string MinistryName { get; set; }
        public string MinistryNameBn { get; set; }
        public decimal Prn { get; set; }
        public decimal Lps { get; set; }
        public decimal Vat { get; set; }
        public decimal Total { get; set; }
        public decimal ReceivePrn { get; set; }
        public decimal ReceiveLps { get; set; }
        public decimal ReceiveVat { get; set; }
        public decimal ReceiveTotal { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string BillcycleCode { get; set; }
        public decimal ArrearAmt { get; set; }
        public decimal ReceiptAmt { get; set; }
        public decimal CurrBill { get; set; }
        public decimal PrevMonth { get; set; }
        public string LocationDesc { get; set; }

        public int Nom { get; set; }
        public string ZoneName { get; set; }
        public string ZoneNameBn { get; set; }
        public string ZoneCode { get; set; }
        public string TotalCustomer { get; set; }
        public decimal TotalMinistryArrear { get; set; }
    }
}

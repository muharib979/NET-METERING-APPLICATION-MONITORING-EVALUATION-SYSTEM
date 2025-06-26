using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class MinistryDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameBn { get; set; }
        public string Code { get; set; }
    }

    public class MinistryDetailsDto 
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public int CustomerNo { get; set; }
        public string LocationCode { get; set; }
        public string LocationDsc { get; set; }
        public string MinistryName { get; set; }
        public string MinistryCode { get; set; }
        public string ZoneName { get; set; }
        public string ZoneNameBN { get; set; }
        public string CircleName { get; set; }
        public string ZoneCode { get; set; }
        //public decimal Prn { get; set; }
        //public decimal Lps { get; set; }
        //public decimal Vat { get; set; }
        //public decimal Total { get; set; }
        public decimal PrevArrearAmt { get; set; }
        public decimal CurrMonthBill { get; set; }
        public decimal CollectionAmt { get; set; }
        public decimal TotalArrearAmt { get; set; }
    }
    public class GetCustomerArrearModelDto
    {
        public int Custid { get; set; }
        public string Consumerno { get; set; }
        public string LocationCode { get; set; }
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
        public string ZoneCode { get; set; }
    }
}

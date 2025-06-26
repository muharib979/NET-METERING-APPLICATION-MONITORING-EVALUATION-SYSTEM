using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Police
{
    public class OnlinePoliceDetailsDTO
    {

        public int Id { get; set; }

        public int CustId { get; set; }
        public int ConsumerNo { get; set; }
        public string Address { get; set; }
        public string CustomerName { get; set; }
        public string LocationCode { get; set; }
        public string LocationDesc { get; set; }
        public string MinistryCode { get; set; }
        public string MinistryName { get; set; }
        public string MinistryNameBn { get; set; }
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? ZoneNameBn { get; set; }
        public decimal CurrVat { get; set; }
        public decimal CurrLps { get; set; }
        public double PrevMonth { get; set; }
        public double CurrBill { get; set; }
        public double PrevArrAmount { get; set; }
        public double ReceiptPrn { get; set; }
        public double ReceiptVat { get; set; }
        public double CurrMonthBillAmount { get; set; }
        public double TotalReceiptAmount { get; set; }
        public double CurrTotalArrAmount { get; set; }
        public decimal CurrReceiptVat { get; set; }
        public decimal CurrReceiptPrincipal { get; set; }
        public decimal CurrPrincipal { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearVat { get; set; }
        public decimal TotalReceiptArrear { get; set; }
        public decimal ReceiptBillMonth { get; set; }
        public decimal ArrearReceiptPrincipal { get; set; }
        public decimal ArrearReceiptVat { get; set; }
        public int HasDepartment { get; set; }
        public int ORDERNO { get; set; }
        public string DeptName { get; set; }
    }
}

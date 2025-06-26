using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class MinistryArrearDto
    {
        public string MinistryNameBn { get; set; }
        public string Name { get; set; }
        public string MinistryCode { get; set; }
        public string DeptNameBn { get; set; }
        public string DeptCode { get; }
        public decimal Bill { get; set; }
        public int HasDepartment { get; set; }
        public string BillCycleCode { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal TotalReceiptArrear { get; set; }
    }
    public class OnlineMinistryArrearDto
    {
        public string ConsumerNo { get; set; }
        public string Address { get; set; }
        public string MinistryName { get; set; }
        public string CustomerName { get; set; }
        public string LocationCode { get; set; }
        public string LocationNameBn { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public decimal ArrearReceiptAmount { get; set; }
        public decimal CurrReceiptVat { get; set; }
        public decimal CurrReceiptAmt { get; set; }
        public decimal TotalArrearAmount { get; set; }
        public double TotalReceiptAmount { get; set; }
        public decimal CurrReceiptPrincipal { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearVat { get; set; }
        public decimal CurrPrin { get; set; }
        public decimal CurrLps { get; set; }
        public decimal CurrVat { get; set; }

        public string UnionPorishadNameBn { get; set; }
        public string UnionPorishadCode { get; set; }

        public decimal TotalReceiptPrincipal { get; set; }
        public decimal TotalReceiptVat { get; set; }
        public string ReceiptBillMonth { get; set; }
        public decimal TotalReceiptArrear { get; set; }
    }
}

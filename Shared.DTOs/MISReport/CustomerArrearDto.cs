using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISReport
{
    public class CustomerArrearDto
    {
        public string Loc { get; set; }
        public string Office { get; set; }
        public string Bg { get; set; }
        public string Bk { get; set; }
        public string ConNo { get; set; }
        public string WlkOr { get; set; }
        public string PvAc { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Addr { get; set; }
        public string Likely { get; set; }
        public string Tarrif { get; set; }
        public int Nom { get; set; }
        public decimal PrincipalArrear { get; set; }
        public decimal LpsArrear { get; set; }
        public decimal VatArrear { get; set; }
        public decimal TotalArrear { get; set; }
        public string BillCycleCode { get; set; }
        public DateTime DiscDate { get; set; }
        public string Status { get; set; }

    }

    public class AllCustomerArrearSummaryDto
    {
        public string Center { get; set; }
        public int Noc { get; set; }
        public double ArrPrn { get; set; }
        public double ArrLps { get; set; }
        public double ArrVat { get; set; }
        public double TotalBill { get; set; }
        public int Order { get; set; }
        public string Loc { get; set; }
        public string Office { get; set; }
    }

    public class PrepaidCustomerArrearBasedOnOffsetDTO 
    {
        public string LocationCode { get; set; }
        public string Office { get; set; }
        public int NoOfConsumer { get; set; }
        public string BillGroup { get; set; }
        public string Book { get; set; }
        public int ConsumerNo { get; set; }
        public string WlkOr { get; set; }
        public string PvAc { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Addr { get; set; }
        public string Likely { get; set; }
        public string Tariff { get; set; }
        public string LastBillMonth { get; set; }
        public decimal PrincipalArrear { get; set; }
        public decimal LpsArrear { get; set; }
        public decimal VatArrear { get; set; }
        public decimal TotalArrear { get; set; }
    }
}

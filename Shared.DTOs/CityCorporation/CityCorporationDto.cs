using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CityCorporation
{
    public class CityCorporationDto
    {
        public int Id { get; set; }
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? ZoneNamebn { get; set; }
        public string? CityNamebn { get; set; }
        public string? PourNamebn { get; set; }
        public string? CityCode { get; set; }
        public string? PouroshovaCode { get; set; }
        public string? CityCorName{ get; set; }
        public string? PourName { get; set; }
        public string? CurrVat { get; set; }
        public string? CurrLps { get; set; }
        public string? CurrPrin { get; set; }
        public string? ValidDate { get; set; }
        public double PrevMonth { get; set; }
        public double CurrBill { get; set; }
        public double PrevArrAmount { get; set; }
        public double ReceiptPrn { get; set; }
        public double ReceiptVat { get; set; }
        public double CurrMonthBillAmount { get; set; }
        public double TotalReceiptAmount { get; set; }
        public double CurrTotalArrAmount { get; set; }
        public double Total_ReceiptAmount { get; set; }
        public decimal  CurrReceiptVat { get; set; }
        public decimal CurrReceiptPrincipal  { get; set; }
        public decimal CurrPrincipal  { get; set; }
        public decimal ArrearLps { get; set; }
        public decimal ArrearPrincipal { get; set; }
        public decimal ArrearVat { get; set; }
        public string? NoOfConsumer { get; set; }
        public decimal TotalReceiptArrear { get; set; }

    }

    public class ZoneWiseCityPouroUnionArrearDto
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string? ZoneNamebn { get; set; }
        //public decimal CitycorparationTotalBill { get; set; }
        //public decimal PourashavaTotalBill { get; set; }
        //public decimal UnionporishodTotalBill { get; set; }
        public decimal CitycorparationPreviousArrear { get; set; }
        public decimal CitycorparationCurrentMonthBill { get; set; }
        public decimal CitycorparationTotalCollection { get; set; }
        public decimal CitycorparationCurrentArrear { get; set; }

        public decimal PourashavaPreviousArrear { get; set; }
        public decimal PourashavaCurrentMonthBill { get; set; }
        public decimal PourashavaTotalCollection { get; set; }
        public decimal PourashavaCurrentArrear { get; set; }

        public decimal UnionporishodPreviousArrear { get; set; }
        public decimal UnionporishodCurrentMonthBill { get; set; }
        public decimal UnionporishodTotalCollection { get; set; }
        public decimal UnionporishodCurrentArrear { get; set; }
    }

    public class ZoneLocationWiseCityPouroUnionDto
    {
        public string LocationCode { get; set; }
        public string LocationNameBn { get; set; }
        //public decimal CitycorporationTotalBill { get; set; }
        //public decimal PouroshovaTotalBill { get; set; }
        //public decimal UnionparisadTotalBill { get; set; }


        public decimal CitycorporationPreviousArrear { get; set; }
        public decimal CitycorporationCurrentMonthBill { get; set; }
        public decimal CitycorporationTotalCollection { get; set; }
        public decimal CitycorporationCurrentArrear { get; set; }


        public decimal PouroshovaPreviousArrear { get; set; }
        public decimal PouroshovaCurrentMonthBill { get; set; }
        public decimal PouroshovaTotalCollection { get; set; }
        public decimal PouroshovaCurrentArrear { get; set; }


        public decimal UnionparishadPreviousArrear { get; set; }
        public decimal UnionparishadCurrentMonthBill { get; set; }
        public decimal UnionparishadTotalCollection { get; set; }
        public decimal UnionparishadCurrentArrear { get; set; }


    }

    public class ZoneWiseUnionPorishodDto 
    {
        public string CustomerNumber { get; set; }
        public string Code { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string UnionPorishodName { get; set; }
        public string TotalCustomer { get; set; }
        public decimal ArrearAmt { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public string locationCode { get; set; }    
        public decimal Prn { get; set; }
        public decimal Lps { get; set; }
        public decimal Vat { get; set; }
        public decimal PrevMonthArrearAmt { get; set; }
        public decimal CurrMonthArrearAmount { get; set; }
        public decimal TotalReceiptAmt { get; set; }
        public decimal TotalArrearAmt { get; set; }
        public string ReceiptBillMonth { get; set; }
        public decimal TotalReceiptArrear { get; set; }
    }

    public class CityCorporationDetailsDTO 
    {
        public string CustomerNo { get; set; }
        public string Customername { get; set; }
        public string Citycorporationcode { get; set; }
        public string CityCorporationName { get; set; }
        public string Pouroshovacode { get; set; }
        public string Pouroshovaname { get; set; }
        //public decimal Prn { get; set; }
        //public decimal Lps { get; set; }
        //public decimal Vat { get; set; }
        //public decimal Totalarrearamount { get; set; }
        public decimal PreviousReceiptArrear { get; set; }
        public decimal CurrentMonthBill { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal CurrentReceiptArrear { get; set; }

        public string? NoOfConsumer { get; set; }


    }
}

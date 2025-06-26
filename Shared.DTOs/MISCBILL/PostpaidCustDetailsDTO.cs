using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class PostpaidCustDetailsDTO
    {
        
        public string? TransID { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public string? ContactName { get; set; }
        public string? Telephone { get; set; }
        public string? Mobile { get; set; }
        public string? ServiceProviders { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerNumber { get; set; }
        public string? FatherName { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? CustomerType { get; set; }
        public string? NidNumber { get; set; }
        public string? MobileNumber { get; set; }
        public string? CustomerAddress { get; set; }
        public string? PowerUtility { get; set; }
        public string? MaxPower { get; set; }
        public string? MeterOwnedBy { get; set; }
        public string? MeterOwner { get; set; }
        public string? TransferDate { get; set; }
        public string? AreaCode { get; set; }
        public string? PrvAcNo { get; set; }
        public string? MeterTypeCode { get; set; }
        public string? MeterNum { get; set; }
        public int? LastReading { get; set; }
        public string? Ldate { get; set; }
        public string? BookNo { get; set; }
        public string? Tin { get; set; }
        public int? CustId { get; set; }
        public int? LastReadingOffPeak { get; set; }
        public string? LastReadingDate { get; set; }
        public string? MeterConditionCode { get; set; }

        public int? LastReadingPeak { get; set; }
        public string? UserCode { get; set; }
        public string? CutOutDate { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? MeterOwnerCode { get; set; }
        public string? DivisionCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? ThanaCode { get; set; }
        public string? CustomerAddr { get; set; }
        public string? BusinessType { get; set; }
        public string? TariffDesc { get; set; }
        public bool? isSanctionLoad { get; set; }
        public int? SanctionedLoad { get; set; }
        public int? TerminalLoss { get; set; }
        public int? LeftSerialNo { get; set; }
        public int? RightSerialNo { get; set; }
        public decimal? Arrear_Amount { get; set; }
        public decimal? ArrearAmount { get; set; }
        public decimal? LastBillReadingSr { get; set; }
        public decimal? LastBillReadingPk { get; set; }
        public decimal? LastBillReadingOfPk { get; set; }
        public string? AppUseName { get; set; }
        public string? AppPassword { get; set; }
        public int? AppIdentity { get; set; }


        public string ToReqXml()
        {
            return $"<xml transID=\"{TransID}\" userName=\"{UserName}\" userPass=\"{UserPass}\" division=\"{DivisionCode}\" district=\"{DistrictCode}\" thana=\"{ThanaCode}\" oldCustomerNo=\"{CustomerNumber}\" oldMeterNo=\"{MeterNum}\" tinno=\"{Tin}\" contactName=\"{ContactName}\" telephone=\"{Telephone}\" mobile=\"{MobileNumber}\" serviceProviders=\"{ServiceProviders}\" customerName=\"{CustomerName}\" customerType=\"{CustomerType}\" nid=\"{NidNumber}\" customerAddress=\"{CustomerAddr}\" powerUtility=\"{PowerUtility}\" maxPower=\"{MaxPower}\" postpaidArrear=\"{ArrearAmount}\"  meterOwnedBy=\"{MeterOwnerCode}\"/>";
        }
    }

    public class PrePaidToPostPaidMOD
    {
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public string? DeptCode { get; set; }
        public string? Fdate { get; set; }
        public string? Tdate { get; set; }
        public string? DbCode { get; set; }
        public string? CreatedBy { get; set; }

        public string ToReqXml()
        {
            return $"<xml deptcode=\"{DeptCode}\" fdate=\"{Fdate}\" tdate=\"{Tdate}\" userName=\"{UserName}\" userPass=\"{UserPass}\"/>";
        }
    }

    public class PrePaidToPostPaidDTO
    {
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public string? TransId { get; set; }

        public string ToRequestXml()
        {
            return $"<xml userPass=\"{UserPass}\" userName=\"{UserName}\" transID=\"{TransId}\"/>";
        }
    }
    public class PrepaidToPostPaidTransferDTO
    {
        public string? UserName { get; set;}
        public string? Password { get; set;}
        public string? PostPaidCustomerNum { get; set;}
        public string? PrePaidCustomerNum { get; set;}
        public string? PrepaidMeterNum { get; set;}
        public string? InstalationDate { get; set;}
    }

    public class PrepaidToPostPaidCustomerDTO
    {
        public string? LOCATION_CODE { get; set;}
        public string? CUSTOMER_NUM { get; set;}
        public string? PREPAID_CUSTOMER_NUMBER { get; set;}
        public string? CUSTOMER_NAME { get; set;}
        public string DB_CODE { get; set;}
        public string? METER_NUM { get; set;}
        public string? REC_STATUS { get; set;}
        public string? BILL_CYCLE_CODE { get; set;}
        public string? CUT_OUT_DATE { get; set;}
        public decimal? LAST_READING { get; set;}
        public decimal? LAST_READING_PK { get; set;}
        public decimal? LAST_READING_OPK { get; set;}
    }

    public class ModPrepaidCustomerDTO
    {
        public string PostPaidCustomerNumber { get; set; }
        public string PrePaidCustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string NidNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public decimal LastReadingSr { get; set; }
        public decimal LastBillReadingSr { get; set; }
        public decimal LastReadingPk { get; set; }
        public decimal LastBillReadingPk { get; set; }
        public decimal LastReadingOpk { get; set; }
        public decimal LastBillReadingOpk { get; set; }
        public decimal ArrearAmount { get; set; }
        public string InstalationDate { get; set; }
    }
    public class LocationWiseCustomerDTO
    {

        //public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string ZoneName { get; set; }
        public string LocationCode { get; set; }
        public int TotalCustomer { get; set; }
   
    }

}

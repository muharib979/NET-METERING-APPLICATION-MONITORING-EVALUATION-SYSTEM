using Core.Domain.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class FireServiceMapperPofile : Profile
    {
        public FireServiceMapperPofile()
        {
            CreateMap<PublicSecurityDivisionMergeDTO, PublicSecurityDivisionDetails>()
           .ForMember(d => d.ID, o => o.MapFrom(s => s.Id))
           .ForMember(d => d.CONSUMER_NO, o => o.MapFrom(s => s.ConsumerNo))
           .ForMember(d => d.CUST_ID, o => o.MapFrom(s => s.CustId))
           .ForMember(d => d.CUSTOMER_NAME, o => o.MapFrom(s => s.CustomerName))
           .ForMember(d => d.LOCATION_CODE, o => o.MapFrom(s => s.LocationCode))
           .ForMember(d => d.MINISTRY_CODE, o => o.MapFrom(s => s.MinistryCode))
           .ForMember(d => d.MINISTRY_NAME, o => o.MapFrom(s => s.MinistryName))
           .ForMember(d => d.MINISTRY_NAME_BN, o => o.MapFrom(s => s.MinistryNameBn))
           .ForMember(d => d.ZONE_CODE, o => o.MapFrom(s => s.ZoneCode))
           .ForMember(d => d.ZONE_NAME, o => o.MapFrom(s => s.ZoneName))
           .ForMember(d => d.ZONE_NAME_BN, o => o.MapFrom(s => s.ZoneNameBn))
           .ForMember(d => d.CURR_PRINCIPAL, o => o.MapFrom(s => s.CurrPrincipal))
           .ForMember(d => d.CURR_LPS, o => o.MapFrom(s => s.CurrLps))
           .ForMember(d => d.CURR_VAT, o => o.MapFrom(s => s.CurrVat))
           .ForMember(d => d.ARREAR_PRICIPAL, o => o.MapFrom(s => s.ArrearPrincipal))
           .ForMember(d => d.ARREAR_LPS, o => o.MapFrom(s => s.ArrearLps))
           .ForMember(d => d.ARREAR_VAT, o => o.MapFrom(s => s.ArrearVat))
           .ForMember(d => d.CURR_RECEIPT_PRINCIPAL, o => o.MapFrom(s => s.CurrReceiptPrincipal))
           .ForMember(d => d.CURR_RECEIPT_VAT, o => o.MapFrom(s => s.CurrReceiptVat))
           .ForMember(d => d.RECEIPT_BILLMONTH, o => o.MapFrom(s => s.ReceiptBillMonth))
           .ForMember(d => d.TOTAL_RECEIPT_ARREAR, o => o.MapFrom(s => s.TotalReceiptArrear))
           .ForMember(d => d.ADDRESS, o => o.MapFrom(s => s.Address))
           .ForMember(d => d.HAS_DEPARTMENT, o => o.MapFrom(s => s.HasDepartment))
           .ReverseMap();
        }


    }
}

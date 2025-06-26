using Core.Domain.CustomeEntity;
using Core.Domain.Ministry;
using Core.Domain.MISCBILL;
using Shared.DTOs.ConsumerBill;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.Ministry;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class CustomerMapperProfile : Profile
    {
        public CustomerMapperProfile()
        {
            CreateMap<CustomerType, CustomerTypeDTO>()
                .ForMember(s => s.CustTypeId, d => d.MapFrom(s => s.CUST_TYPE_ID))
                .ForMember(s => s.CreateDate, d => d.MapFrom(s => s.CREATE_DATE))
                .ForMember(s => s.CreateBy, d => d.MapFrom(s => s.CREATE_BY))
                .ForMember(s => s.CustTypeDesc, d => d.MapFrom(s => s.CUST_TYPE_DESC))
                .ForMember(s => s.CustTypeCode, d => d.MapFrom(s => s.CUST_TYPE_CODE))
                .ForMember(s => s.UpdateBy, d => d.MapFrom(s => s.UPDATE_BY))
                .ForMember(s => s.Remarks, d => d.MapFrom(s => s.REMARKS))
                .ForMember(s => s.Status, d => d.MapFrom(s => s.STATUS))
          .ReverseMap();

            CreateMap<CustomerCategory, CustomerCategoryDTO>()
               .ForMember(s => s.UsasCatId, d => d.MapFrom(s => s.USAGE_CAT_ID))
               .ForMember(s => s.CreateDate, d => d.MapFrom(s => s.CREATE_DATE))
               .ForMember(s => s.CreateBy, d => d.MapFrom(s => s.CREATE_BY))
               .ForMember(s => s.UsasCatCode, d => d.MapFrom(s => s.USAGE_CAT_CODE))
               .ForMember(s => s.UsasCatDesc, d => d.MapFrom(s => s.USAGE_CAT_DESC))
               .ForMember(s => s.UpdateDate, d => d.MapFrom(s => s.UPDATE_DATE))
               .ForMember(s => s.UpdateBy, d => d.MapFrom(s => s.UPDATE_BY))
               .ForMember(s => s.Remarks, d => d.MapFrom(s => s.REMARKS))
               .ForMember(s => s.Status, d => d.MapFrom(s => s.STATUS))
               .ReverseMap();

            CreateMap<CustomerTariff, CustomerTariffDto>()
            .ForMember(s => s.TariffId, d => d.MapFrom(s => s.TARIFF_ID))
            .ForMember(s => s.CreateDate, d => d.MapFrom(s => s.CREATE_DATE))
            .ForMember(s => s.TariffDesc, d => d.MapFrom(s => s.TARIFF_DESC))
            .ForMember(s => s.TariffCode, d => d.MapFrom(s => s.TARIFF_CODE))
            .ForMember(s => s.Status, d => d.MapFrom(s => s.STATUS))
            .ReverseMap();

            CreateMap<ConsumerBill, ConsumerBillDTO>()
            .ForMember(s => s.BillNumber, d => d.MapFrom(s => s.BILL_NO))
            .ForMember(s => s.CustomerNumber, d => d.MapFrom(s => s.CUSTOMER_NUM))
            .ForMember(s => s.CustomerName, d => d.MapFrom(s => s.CUST_NAME))
            .ForMember(s => s.LocationCode, d => d.MapFrom(s => s.LOCATION_CODE))
            .ForMember(s => s.TotalAmount, d => d.MapFrom(s => s.TOTAL_BILL_AMOUNT))
            .ForMember(s => s.VatAmount, d => d.MapFrom(s => s.TOT_VAT_AMOUNT))
            .ForMember(s => s.PrincipleAmount, d => d.MapFrom(s => s.TOTAL_PRINCIPAL_AMOUNT))
            .ForMember(s => s.CustomerAddress, d => d.MapFrom(s => s.CUST_ADDRESS))
            .ForMember(s => s.Phone, d => d.MapFrom(s => s.PHONE))
            .ForMember(s => s.Paid, d => d.MapFrom(s => s.PAY_STATUS))
            .ReverseMap();

            CreateMap<CustomerDetails, CustomerDetailsPenaltyBillPrepaidDTO>()
       .ForMember(s => s.CustId, d => d.MapFrom(s => s.CUST_ID))
       .ForMember(s => s.CustomerNumber, d => d.MapFrom(s => s.CustomerNumber))
       .ForMember(s => s.CustomerName, d => d.MapFrom(s => s.CUSTOMER_NAME))
       .ForMember(s => s.LocationCode, d => d.MapFrom(s => s.LOCATION_CODE))
       .ForMember(s => s.LocationName, d => d.MapFrom(s => s.LOCATION_NAME))
       .ForMember(s => s.SanctionedLoad, d => d.MapFrom(s => s.SanctionedLoad))
       .ForMember(s => s.AreaCode, d => d.MapFrom(s => s.AREA_CODE))
       .ForMember(s => s.TariffDesc, d => d.MapFrom(s => s.TARIFF_DESC))
       .ForMember(s => s.MeterNum, d => d.MapFrom(s => s.METER_NUM))
       .ForMember(s => s.WlkOrd, d => d.MapFrom(s => s.Wlk_ord))
       .ForMember(s => s.BillGroup, d => d.MapFrom(s => s.BILL_GROUP))
       .ForMember(s => s.BookNo, d => d.MapFrom(s => s.BOOK_NO))
       .ForMember(s => s.PrvAcNo, d => d.MapFrom(s => s.PRV_AC_NO))
       .ForMember(s => s.CustomerAddr, d => d.MapFrom(s => s.CUSTOMER_ADDR))
       .ForMember(s => s.MeterConditionCode, d => d.MapFrom(s => s.METER_CODINTION_CODE))
       .ForMember(s => s.MeterConditionDesc, d => d.MapFrom(s => s.METER_CODINTION_DESC))
       .ForMember(s => s.MeterTypeCode, d => d.MapFrom(s => s.METER_TYPE_Code))
       .ForMember(s => s.MeterTypeDesc, d => d.MapFrom(s => s.METER_TYPE_DESC))
       .ForMember(s => s.BillCycleCode, d => d.MapFrom(s => s.bill_cycle_code))
       .ForMember(s => s.BusinessType, d => d.MapFrom(s => s.BUSINESS_TYPE))
       .ForMember(s => s.BusinessTypeCode, d => d.MapFrom(s => s.BUSINESS_TYPE_CODE))
       .ForMember(s => s.FatherName, d => d.MapFrom(s => s.Father_Name))
       .ReverseMap();

            CreateMap<GetCustomerArrearModel, GetCustomerArrearModelDto>()
        .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.CUSTOMER_NAME))
        .ForMember(d => d.Address, o => o.MapFrom(s => s.ADDRESS))
        .ForMember(d => d.Custid, o => o.MapFrom(s => s.CUST_ID))
        .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
        .ForMember(d => d.Prn, o => o.MapFrom(s => s.PRN))
        .ForMember(d => d.Lps, o => o.MapFrom(s => s.LPS))
        .ForMember(d => d.Total, o => o.MapFrom(s => s.TOTAL))
        .ForMember(d => d.ReceivePrn, o => o.MapFrom(s => s.RECEIVE_PRN))
        .ForMember(d => d.ReceiveLps, o => o.MapFrom(s => s.RECEIVE_LPS))
        .ForMember(d => d.ReceiveVat, o => o.MapFrom(s => s.RECEIVE_VAT))
        .ForMember(d => d.ReceiveTotal, o => o.MapFrom(s => s.RECEIVE_TOTAL))
        .ForMember(d => d.BillcycleCode, o => o.MapFrom(s => s.BILL_CYCLE_CODE))
        .ForMember(d => d.ArrearAmt, o => o.MapFrom(s => s.ARREAR_AMT))
        .ForMember(d => d.ReceiptAmt, o => o.MapFrom(s => s.RECEIPT_AMT))
        .ForMember(d => d.CurrBill, o => o.MapFrom(s => s.CURR_BILL))
        .ForMember(d => d.PrevMonth, o => o.MapFrom(s => s.PREV_MONTH))
        .ForMember(d => d.LocationDesc, o => o.MapFrom(s => s.LOCATION_DESC))
        .ForMember(d => d.Consumerno, o => o.MapFrom(s => s.CONSUMER_NO))
        .ForMember(d => d.Nom, o => o.MapFrom(s => s.NOM))
        .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_NAME))
        .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
        .ReverseMap();

            CreateMap<UserDetails, UserInfoDto>()
      .ForMember(d => d.CustomerNumber, o => o.MapFrom(s => s.CUST_NUM))
      .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
      .ForMember(d => d.Month, o => o.MapFrom(s => s.MONTH))
      .ForMember(d => d.MonthArr, o => o.MapFrom(s => s.MONTH_ARR))
      .ReverseMap();

        }
    }
}

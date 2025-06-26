using Core.Domain.Agricultures;
using Shared.DTOs.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class AgricultureMapperProfile: Profile
    {
        public AgricultureMapperProfile() 
        {
            CreateMap<OnlineAgricultureMergeDTO, AgricultureArrearModel>()
                .ForMember(d => d.CUST_ID, o => o.MapFrom(s => s.CustId))
                .ForMember(d => d.CONSUMER_NO, o => o.MapFrom(s => s.ConsumerNo))
                .ForMember(d => d.CUSTOMER_NAME, o => o.MapFrom(s => s.CustomerName))
                .ForMember(d => d.KRISHI_DEPT_NAME_BN, o => o.MapFrom(s => s.KrishiDeptNameBn))
                .ForMember(d => d.LOCATION_NAME_BN, o => o.MapFrom(s => s.LocationNameBn))
                .ForMember(d => d.IS_KRISHI, o => o.MapFrom(s => s.IsKrishi))
                .ForMember(d => d.IS_POULTRY, o => o.MapFrom(s => s.IsPoultry))
                .ForMember(d => d.CON_EXTG_NUM, o => o.MapFrom(s => s.ConExtgNum))
                .ForMember(d => d.LOCATION_NAME, o => o.MapFrom(s => s.LocationName))
                .ForMember(d => d.CURR_RECEIPT_PRINCIPAL, o => o.MapFrom(s => s.CurrReceiptPrincipal))
                .ForMember(d => d.CURR_RECEIPT_VAT, o => o.MapFrom(s => s.CurrReceiptVat))
                .ForMember(d => d.ARREAR_PRICIPAL, o => o.MapFrom(s => s.ArrearPrincipal))
                .ForMember(d => d.ARREAR_LPS, o => o.MapFrom(s => s.ArrearLps))
                .ForMember(d => d.ARREAR_VAT, o => o.MapFrom(s => s.ArrearVat))
                .ForMember(d => d.CURR_PRINCIPAL, o => o.MapFrom(s => s.CurrPrincipal))
                .ForMember(d => d.CURR_LPS, o => o.MapFrom(s => s.CurrLps))
                .ForMember(d => d.CURR_VAT, o => o.MapFrom(s => s.CurrVat))
                .ForMember(d => d.Total_Receipt_Arrear, o => o.MapFrom(s => s.TotalReceiptArrear))
                .ForMember(d => d.ARREAR_RECEIPT_PRINCIPAL, o => o.MapFrom(s => s.ArrearReceiptPrincipal))
                .ForMember(d => d.ARREAR_RECEIPT_VAT, o => o.MapFrom(s => s.ArrearReceiptVat))
                .ForMember(d => d.ZONE_CODE, o => o.MapFrom(s => s.ZoneCode))
                .ForMember(d => d.LOCATION_CODE, o => o.MapFrom(s => s.LocationCode))
                .ReverseMap();

            CreateMap<OnlineAgricultureLedgerMergeDTO, OnlineAgricultureLedgerModel>()
                .ForMember(d => d.CUST_ID, o => o.MapFrom(s => s.CustId))
                .ForMember(d => d.CONSUMER_NO, o => o.MapFrom(s => s.ConsumerNo))
                .ForMember(d => d.CUSTOMER_NAME, o => o.MapFrom(s => s.CustomerName))
                .ForMember(d => d.KRISHI_DEPT_NAME_BN, o => o.MapFrom(s => s.KrishiDeptNameBn))
                .ForMember(d => d.LOCATION_NAME_BN, o => o.MapFrom(s => s.LocationNameBn))
                .ForMember(d => d.IS_KRISHI, o => o.MapFrom(s => s.IsKrishi))
                .ForMember(d => d.IS_POULTRY, o => o.MapFrom(s => s.IsPoultry))
                .ForMember(d => d.CON_EXTG_NUM, o => o.MapFrom(s => s.ConExtgNum))
                .ForMember(d => d.LOCATION_NAME, o => o.MapFrom(s => s.LocationName))
                .ForMember(d => d.CURR_RECEIPT_PRINCIPAL, o => o.MapFrom(s => s.CurrReceiptPrincipal))
                .ForMember(d => d.CURR_RECEIPT_VAT, o => o.MapFrom(s => s.CurrReceiptVat))
                .ForMember(d => d.ARREAR_PRICIPAL, o => o.MapFrom(s => s.ArrearPrincipal))
                .ForMember(d => d.ARREAR_LPS, o => o.MapFrom(s => s.ArrearLps))
                .ForMember(d => d.ARREAR_VAT, o => o.MapFrom(s => s.ArrearVat))
                .ForMember(d => d.CURR_PRINCIPAL, o => o.MapFrom(s => s.CurrPrincipal))
                .ForMember(d => d.CURR_LPS, o => o.MapFrom(s => s.CurrLps))
                .ForMember(d => d.CURR_VAT, o => o.MapFrom(s => s.CurrVat))
                .ForMember(d => d.Total_Receipt_Arrear, o => o.MapFrom(s => s.TotalReceiptArrear))
                .ForMember(d => d.ARREAR_RECEIPT_PRINCIPAL, o => o.MapFrom(s => s.ArrearReceiptPrincipal))
                .ForMember(d => d.ARREAR_RECEIPT_VAT, o => o.MapFrom(s => s.ArrearReceiptVat))
                .ForMember(d => d.ZONE_CODE, o => o.MapFrom(s => s.ZoneCode))
                .ReverseMap();
        }
    }
}

using Core.Domain.NonBengali;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class NonBengaliMapperProfile : Profile
    {
        public NonBengaliMapperProfile()
        {
            CreateMap<NonBengalis, NonBengaliDTOs>()
                .ForMember(o => o.NonbengaliNameCode, e => e.MapFrom(s => s.NON_BANGLI_NAME_CODE))
                .ForMember(o => o.NameBn, e => e.MapFrom(s => s.NAMEBN))
                .ForMember(o => o.LocationCode, e => e.MapFrom(s => s.LOCATION_CODE))
                .ForMember(o => o.LocationNameBn, e => e.MapFrom(s => s.LOCATION_NAMEBN))
                .ForMember(o => o.MeterCount, e => e.MapFrom(s => s.METER_COUNT))
                .ForMember(o => o.Prn, e => e.MapFrom(s => s.PRN))
                .ForMember(o => o.Lps, e => e.MapFrom(s => s.LPS))
                .ForMember(o => o.Vat, e => e.MapFrom(s => s.VAT))
                //.ForMember(o => o.PreviousArrear, e => e.MapFrom(s => s.PREV_ARREAR_AMT))
                //.ForMember(o => o.CurrentMonthBill, e => e.MapFrom(s => s.CURR_MONTH_BILL))
                //.ForMember(o => o.TotalCollection, e => e.MapFrom(s => s.COLLECTION_AMOUNT))
                //.ForMember(o => o.CurrentArrear, e => e.MapFrom(s => s.TOTAL_ARREAR_AMOUNT))
                .ForMember(o => o.TotalArrear, e => e.MapFrom(s => s.TOTAL_ARREAR))
                .ForMember(o => o.ZoneCode, e => e.MapFrom(s => s.ZONE_CODE))
                .ForMember(o => o.ZoneName, e => e.MapFrom(s => s.ZONE_NAME))
                .ForMember(o => o.CustomerNo, e => e.MapFrom(s => s.CUSTOMER_NO))
                .ReverseMap();

            CreateMap<NonBengaliSummary, NonBengaliSummaryDTO>()
                .ForMember(o => o.NonBengaliNameCode, e => e.MapFrom(s => s.NON_BANGLI_NAME_CODE))
                .ForMember(o => o.NonBengaliNameBn, e => e.MapFrom(s => s.NAMEBN))
                .ForMember(o => o.LocationCode, e => e.MapFrom(s => s.LOCATION_CODE))
                .ForMember(o => o.LocationName, e => e.MapFrom(s => s.LOCATION_NAMEBN))
                .ForMember(o => o.MeterCount, e => e.MapFrom(s => s.METER_COUNT))
                .ForMember(o => o.Prn, e => e.MapFrom(s => s.PRN))
                .ForMember(o => o.Lps, e => e.MapFrom(s => s.LPS))
                .ForMember(o => o.TotalArrear, e => e.MapFrom(s => s.TOTAL_ARREAR))
                .ForMember(o => o.IsStatic, e => e.MapFrom(s => s.IS_SATATIC))
                .ForMember(o => o.BillCycleCode, e => e.MapFrom(s => s.BILL_CYCLE_CODE))
                .ReverseMap();

            CreateMap<OnlineNonBengaliSummaryMerge, OnlineNonBengaliSummaryMergeDTO>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.ADDRESS))
                .ForMember(d => d.ConsumerNo, o => o.MapFrom(s => s.CONSUMER_NO))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.CUSTOMER_NAME))
                .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))

                .ForMember(d => d.TotalArrearAmount, o => o.MapFrom(s => s.ARREAR_AMT))
                .ForMember(d => d.NonBengaliCampCode, o => o.MapFrom(s => s.NON_BENGALI_CAMP_CODE))
                //.ForMember(d => d.NonBengaliName, o => o.MapFrom(s => s.NONBENGALI_NAME))
                .ForMember(d => d.NonBengaliNameBn, o => o.MapFrom(s => s.NONBENGALI_NAMEBN))
                .ForMember(d => d.TotalArrearAmount, o => o.MapFrom(s => s.ARREAR_AMT))
                .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
                .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_NAME))
                //.ForMember(d => d.ReceiptAmt, o => o.MapFrom(s => s.RECEIPT_AMT))
                .ForMember(d => d.ArrearReceiptAmount, o => o.MapFrom(s => s.ARREAR_RECEIPT_AMOUNT))
                .ForMember(d => d.CurrReceiptVat, o => o.MapFrom(s => s.CURR_RECEIPT_VAT))
                .ForMember(d => d.CurrReceiptAmt, o => o.MapFrom(s => s.CURR_RECEIPT_AMT))

              .ForMember(d => d.ArrearReceiptAmount, o => o.MapFrom(s => s.ARREAR_RECEIPT_AMOUNT))
              //.ForMember(d => d.TotalReceiptAmount, o => o.MapFrom(s => s.TOTAL_RECEIPT_AMOUNT))
              .ForMember(d => d.CurrReceiptVat, o => o.MapFrom(s => s.CURR_RECEIPT_VAT))
              .ForMember(d => d.CurrReceiptPrincipal, o => o.MapFrom(s => s.CURR_RECEIPT_PRINCIPAL))
              .ForMember(d => d.CurrVat, o => o.MapFrom(s => s.CURR_VAT))
              .ForMember(d => d.CurrLps, o => o.MapFrom(s => s.CURR_LPS))
              .ForMember(d => d.CurrPrin, o => o.MapFrom(s => s.CURR_PRINCIPAL))
              .ForMember(d => d.CurrPrincipal, o => o.MapFrom(s => s.CURR_PRINCIPAL))
              .ForMember(d => d.ArrearLps, o => o.MapFrom(s => s.ARREAR_LPS))
              .ForMember(d => d.ArrearVat, o => o.MapFrom(s => s.ARREAR_VAT))
              .ForMember(d => d.ArrearPrincipal, o => o.MapFrom(s => s.ARREAR_PRICIPAL))
              .ForMember(d => d.TotalReceiptPrincipal, o => o.MapFrom(s => s.TOTAL_RECEIPT_PRINCIPAL))
              .ForMember(d => d.TotalReceiptVat, o => o.MapFrom(s => s.TOTAL_RECEIPT_VAT))
              .ReverseMap();

            CreateMap<OnlineNonBengaliSummaryMergeDTO, OnlineNonBengaliSummaryDTO>()
                .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LocationCode))
                .ForMember(d => d.MeterCount, o => o.MapFrom(s => s.MeterCount))
                .ForMember(d => d.Prn, o => o.MapFrom(s => s.CurrPrin))
                .ForMember(d => d.Lps, o => o.MapFrom(s => s.CurrLps))
                .ForMember(d => d.Vat, o => o.MapFrom(s => s.CurrVat))
                .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
                .ForMember(d => d.LocationDesc, o => o.MapFrom(s => s.LocationDesc))
                .ForMember(d => d.LocationNameBn, o => o.MapFrom(s => s.LocationNameBn))
                .ForMember(d => d.BillCycleCode, o => o.MapFrom(s => s.BillCycleCode))
                .ForMember(d => d.NonBengaliCampCode, o => o.MapFrom(s => s.NonBengaliCampCode))
                .ForMember(d => d.NonBengaliNameBn, o => o.MapFrom(s => s.NonBengaliNameBn))
                .ForMember(d => d.CurrPrin, o => o.MapFrom(s => s.CurrPrin))
                .ForMember(d => d.CurrLps, o => o.MapFrom(s => s.CurrLps))
                .ForMember(d => d.CurrVat, o => o.MapFrom(s => s.CurrVat))
                .ForMember(d => d.ConsumerNo, o => o.MapFrom(s => s.ConsumerNo))
                .ReverseMap();


        }
    }
}

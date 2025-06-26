using Core.Domain.CityCorporation;
using Core.Domain.MinistryCustomer;
using Core.Domain.UnionPorishad;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class CityCorporationMapperProfile:Profile
    {
        public CityCorporationMapperProfile() 
        {
            CreateMap<CityCorporations, CityCorporationDto>()

              .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
              .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
              .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_NAME))
              .ForMember(d => d.ZoneNamebn, o => o.MapFrom(s => s.ZONE_NAMEBN))
              .ForMember(d => d.CityNamebn, o => o.MapFrom(s => s.CITY_NAMEBN))
              .ForMember(d => d.PourNamebn, o => o.MapFrom(s => s.POUR_NAMEBN))
              .ForMember(d => d.CityCode, o => o.MapFrom(s => s.CITY_CODE))
              .ForMember(d => d.PouroshovaCode, o => o.MapFrom(s => s.POUROSHOVA_CODE))
              .ForMember(d => d.CityCorName, o => o.MapFrom(s => s.CITY_COR_NAME))
              .ForMember(d => d.PourName, o => o.MapFrom(s => s.POUR_NAME))
              .ForMember(d => d.CurrVat, o => o.MapFrom(s => s.CURR_VAT))
              .ForMember(d => d.CurrLps, o => o.MapFrom(s => s.CURR_LPS))
              .ForMember(d => d.CurrPrin, o => o.MapFrom(s => s.CURR_PRIN))
              .ForMember(d => d.ValidDate, o => o.MapFrom(s => s.VALID_DATE))
              .ForMember(d => d.PrevMonth, o => o.MapFrom(s => s.PREV_MONTH))
              .ForMember(d => d.CurrBill, o => o.MapFrom(s => s.CURR_BILL))
              .ForMember(d => d.PrevArrAmount, o => o.MapFrom(s => s.PREV_ARR_AMOUNT))
              .ForMember(d => d.ReceiptPrn, o => o.MapFrom(s => s.RECEIPT_PRN))
              .ForMember(d => d.ReceiptVat, o => o.MapFrom(s => s.RECEIPT_VAT))
              .ForMember(d => d.CurrMonthBillAmount, o => o.MapFrom(s => s.CURR_MONTH_BILL_AMOUNT))
              .ForMember(d => d.CurrTotalArrAmount, o => o.MapFrom(s => s.CURR_TOTAL_ARR_AMOUNT))
              .ForMember(d => d.TotalReceiptAmount, o => o.MapFrom(s => s.TOTAL_RECEIPT_AMOUNT))
              .ForMember(d => d.CurrReceiptVat, o => o.MapFrom(s => s.CURR_RECEIPT_VAT))
              .ForMember(d => d.CurrReceiptPrincipal, o => o.MapFrom(s => s.CURR_RECEIPT_PRINCIPAL))
              .ForMember(d => d.CurrVat, o => o.MapFrom(s => s.CURR_VAT))
              .ForMember(d => d.CurrLps, o => o.MapFrom(s => s.CURR_LPS))
              .ForMember(d => d.CurrPrin, o => o.MapFrom(s => s.CURR_PRINCIPAL))
              .ForMember(d => d.ArrearLps, o => o.MapFrom(s => s.ARREAR_LPS))
              .ForMember(d => d.ArrearVat, o => o.MapFrom(s => s.ARREAR_VAT))
              .ForMember(d => d.ArrearPrincipal, o => o.MapFrom(s => s.ARREAR_PRICIPAL))
              .ForMember(d => d.NoOfConsumer, o => o.MapFrom(s => s.NO_OF_CONSUMER))
              .ForMember(d => d.TotalReceiptArrear, o => o.MapFrom(s => s.TOTAL_RECEIPT_ARREAR))
              .ReverseMap();

            CreateMap<CityCorporationData, CityCorporationDataDto>()

             .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
             .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
               .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAME))
                 .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
                  .ReverseMap();

            CreateMap<ZoneWiseCityPouroUnionArrear, ZoneWiseCityPouroUnionArrearDto>()
             .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
             .ForMember(d => d.ZoneName, o => o.MapFrom(s=> s.ZONE_NAME))
             .ForMember(d => d.ZoneNamebn,o => o.MapFrom(s=>s.ZONE_NAMEBN))
                //.ForMember(d => d.CitycorparationTotalBill,o => o.MapFrom(s=>s.CITYCORPARATION_TOTAL_BILL))
                //.ForMember(d => d.PourashavaTotalBill,o => o.MapFrom(s=>s.POURASHAVA_TOTAL_BILL))
                //.ForMember(d => d.UnionporishodTotalBill,o => o.MapFrom(s=>s.UNIONPORISHOD_TOTAL_BILL))
                .ForMember(d => d.CitycorparationPreviousArrear, o => o.MapFrom(s => s.CITYCORPARATION_PREVIOUS_ARREAR))
                .ForMember(d => d.CitycorparationCurrentMonthBill, o => o.MapFrom(s => s.CITYCORPARATION_CURRENT_MONTH_BILL))
                .ForMember(d => d.CitycorparationTotalCollection, o => o.MapFrom(s => s.CITYCORPARATION_TOTAL_COLLECTION))
                .ForMember(d => d.CitycorparationCurrentArrear, o => o.MapFrom(s => s.CITYCORPARATION_CURRENT_ARREAR))

                .ForMember(d => d.PourashavaPreviousArrear, o => o.MapFrom(s => s.POURASHAVA_PREVIOUS_ARREAR))
                .ForMember(d => d.PourashavaCurrentMonthBill, o => o.MapFrom(s => s.POURASHAVA_CURRENT_MONTH_BILL))
                .ForMember(d => d.PourashavaTotalCollection, o => o.MapFrom(s => s.POURASHAVA_TOTAL_COLLECTION))
                .ForMember(d => d.PourashavaCurrentArrear, o => o.MapFrom(s => s.POURASHAVA_CURRENT_ARREAR))

                .ForMember(d => d.UnionporishodPreviousArrear, o => o.MapFrom(s => s.UNIONPORISHOD_PREVIOUS_ARREAR))
                .ForMember(d => d.UnionporishodCurrentMonthBill, o => o.MapFrom(s => s.UNIONPORISHOD_CURRENT_MONTH_BILL))
                .ForMember(d => d.UnionporishodTotalCollection, o => o.MapFrom(s => s.UNIONPORISHOD_TOTAL_COLLECTION))
                .ForMember(d => d.UnionporishodCurrentArrear, o => o.MapFrom(s => s.UNIONPORISHOD_CURRENT_ARREAR))
                .ReverseMap();

            CreateMap<ZoneLocationWiseCityPouroUnion, ZoneLocationWiseCityPouroUnionDto>()
                .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
                .ForMember(d => d.LocationNameBn, o => o.MapFrom(s => s.LOCATION_NAMEBN))
                .ForMember(d => d.CitycorporationPreviousArrear, o => o.MapFrom(s => s.CITYCORPORATION_PREVIOUS_ARREAR))
                .ForMember(d => d.CitycorporationCurrentMonthBill, o => o.MapFrom(s => s.CITYCORPORATION_CURRENT_MONTH_BILL))
                .ForMember(d => d.CitycorporationTotalCollection, o => o.MapFrom(s => s.CITYCORPORATION_TOTAL_COLLECTION))
                .ForMember(d => d.CitycorporationCurrentArrear, o => o.MapFrom(s => s.CITYCORPORATION_CURRENT_ARREAR))

                .ForMember(d => d.PouroshovaPreviousArrear, o => o.MapFrom(s => s.POUROSHOVA_PREVIOUS_ARREAR))
                .ForMember(d => d.PouroshovaCurrentMonthBill, o => o.MapFrom(s => s.POUROSHOVA_CURRENT_MONTH_BILL))
                .ForMember(d => d.PouroshovaTotalCollection, o => o.MapFrom(s => s.POUROSHOVA_TOTAL_COLLECTION))
                .ForMember(d => d.PouroshovaCurrentArrear, o => o.MapFrom(s => s.POUROSHOVA_CURRENT_ARREAR))

                .ForMember(d => d.UnionparishadPreviousArrear, o => o.MapFrom(s => s.UNIONPARISHAD_PREVIOUS_ARREAR))
                .ForMember(d => d.UnionparishadCurrentMonthBill, o => o.MapFrom(s => s.UNIONPARISHAD_CURRENT_MONTH_BILL))
                .ForMember(d => d.UnionparishadTotalCollection, o => o.MapFrom(s => s.UNIONPARISHAD_TOTAL_COLLECTION))
                .ForMember(d => d.UnionparishadCurrentArrear, o => o.MapFrom(s => s.UNIONPARISHAD_CURRENT_ARREAR))
                .ReverseMap();

            CreateMap<Pouroshova, DropdownResultForStringKey>()

                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAME))
                .ReverseMap();

            CreateMap<UnionParishad, DropdownResultForStringKey>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAME))
                .ReverseMap();

            CreateMap<MinistryDepartment, DropdownResultForStringKey>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAME))
                .ReverseMap();

            CreateMap<OnlineCityPourMergeData, OnlineCityPourMergeDTO>()
                .ForMember(d=>d.Address,o=>o.MapFrom(s=>s.ADDRESS))
                .ForMember(d=>d.ConsumerNo,o=>o.MapFrom(s=>s.CONSUMER_NO))
                .ForMember(d=>d.CustomerName,o=>o.MapFrom(s=>s.CUSTOMER_NAME))
                .ForMember(d=>d.LocationCode,o=>o.MapFrom(s=>s.LOCATION_CODE))

                .ForMember(d=>d.TotalArrearAmount,o=>o.MapFrom(s=>s.ARREAR_AMT))
                .ForMember(d=>d.CityCorporationCode,o=>o.MapFrom(s=>s.CITYCORPORATION_CODE))
                .ForMember(d=>d.CityCorporationName,o=>o.MapFrom(s=>s.City_Cor_NAME))
                .ForMember(d=>d.PouroName,o=>o.MapFrom(s=>s.POUR_NAME))
                .ForMember(d=>d.PouroshovaCode,o=>o.MapFrom(s=>s.POUROSHOVA_CODE))
                 .ForMember(d => d.TotalArrearAmount, o => o.MapFrom(s => s.ARREAR_AMT))
                .ForMember(d=>d.CityCorporationCode,o=>o.MapFrom(s=>s.CITYCORPORATION_CODE))
                .ForMember(d=>d.CityCorporationName,o=>o.MapFrom(s=>s.City_Cor_NAME))
                .ForMember(d=>d.PouroName,o=>o.MapFrom(s=>s.POUR_NAME))
                .ForMember(d => d.PouroshovaCode, o => o.MapFrom(s => s.POUROSHOVA_CODE))
                .ForMember(d=>d.ZoneCode,o=>o.MapFrom(s=>s.ZONE_CODE))
                .ForMember(d=>d.ZoneName,o=>o.MapFrom(s=>s.ZONE_NAME))
                .ForMember(d=>d.ReceiptAmt,o=>o.MapFrom(s=>s.RECEIPT_AMT))
                .ForMember(d=>d.ArrearReceiptAmount,o=>o.MapFrom(s=>s.ARREAR_RECEIPT_AMOUNT))
                .ForMember(d=>d.CurrReceiptVat,o=>o.MapFrom(s=>s.CURR_RECEIPT_VAT))
                .ForMember(d=>d.CurrReceiptAmt,o=>o.MapFrom(s=>s.CURR_RECEIPT_AMT))
              
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
              .ForMember(d => d.PouroNameBn, o => o.MapFrom(s => s.POUR_NAMEBN))
              .ForMember(d => d.CityCorporationNameBn, o => o.MapFrom(s => s.City_Cor_NAMEBN))
              .ForMember(d => d.TotalReceiptPrincipal, o => o.MapFrom(s => s.TOTAL_RECEIPT_PRINCIPAL))
              .ForMember(d => d.TotalReceiptVat, o => o.MapFrom(s => s.TOTAL_RECEIPT_VAT))
              .ForMember(d=>d.ReceiptBillMonth, o=>o.MapFrom(s =>s.RECEIPT_BILLMONTH))
              .ForMember(d=>d.PrvReceiptPrincipal, o=>o.MapFrom(s=>s.PRV_RECEIPT_PRINCIPAL))
              .ForMember(d=>d.PrvReceiptVat, o=>o.MapFrom(s=>s.PRV_RECEIPT_VAT))
              .ForMember(d=>d.PrvReceiptAmt, o=>o.MapFrom(s=>s.PRV_RECEIPT_AMT))
              .ForMember(d=>d.OrderNo, o=>o.MapFrom(s=>s.ORDER_NO))
              .ForMember(d=>d.TotalReceiptArrear, o=>o.MapFrom(s=>s.Total_Receipt_Arrear))
              .ReverseMap();

            CreateMap<OnlineUnionPorisadMergeDataDto,ZoneWiseUnionPorishodDto>()
                .ForMember(d => d.locationCode, o => o.MapFrom(s => s.LocationCode))
                .ForMember(d => d.UnionPorishodName, o => o.MapFrom(s => s.UnionPorishadNameBn))
                .ForMember(d => d.CustomerNumber, o => o.MapFrom(s => s. ConsumerNo))
                //.ForMember(d => d.Prn, o => o.MapFrom(s => (s.CurrPrin + s.ArrearPrincipal)))
                //.ForMember(d => d.Vat, o => o.MapFrom(s => (s.CurrVat + s.ArrearVat)))
                //.ForMember(d => d.Lps, o => o.MapFrom(s => (s.CurrLps + s.ArrearLps)))

                .ForMember(d => d.PrevMonthArrearAmt, o => o.MapFrom(s => ((s.ArrearPrincipal + s.ArrearLps + s.ArrearVat) - s.TotalReceiptArrear)))
                .ForMember(d => d.CurrMonthArrearAmount, o => o.MapFrom(s => (s.CurrPrin + s.CurrLps + s.CurrVat)))
                .ForMember(d => d.TotalReceiptAmt, o => o.MapFrom(s => (s.CurrReceiptPrincipal + s.CurrReceiptVat)))

                .ForMember(d => d.ArrearAmt, o => o.MapFrom(s => s.TotalArrearAmount))
                .ForMember(d => d.locationCode, o => o.MapFrom(s => s.LocationCode))
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.LocationNameBn))
                .ForMember(d => d.TotalReceiptArrear, o => o.MapFrom(s => s.TotalReceiptArrear))
                .ReverseMap();
                
                
                //.ForMember(d=>d., o =>o.MapFrom(s=>s.locationCode))

        }
    }
}

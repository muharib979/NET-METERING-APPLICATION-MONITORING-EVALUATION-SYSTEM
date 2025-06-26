using Core.Domain.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class MinistryMapperProfile: Profile
    {
        public MinistryMapperProfile()
        {
            //CreateMap<MinistryArrear, MinistryArrearDto>()
            //.ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
            //.ForMember(d => d.CustId, o => o.MapFrom(s => s.CUST_ID))
            //.ForMember(d => d.ConsumerNo, o => o.MapFrom(s => s.CONSUMER_NO))
            //.ForMember(d => d.address, o => o.MapFrom(s => s.ADDRESS))
            //.ForMember(d => d.CustomerName, o => o.MapFrom(s => s.CUSTOMER_NAME))
            //.ForMember(d => d.RunBillCycleCode, o => o.MapFrom(s => s.RUN_BILL_CYCLE_CODE))
            //.ForMember(d => d.BookNo, o => o.MapFrom(s => s.BOOK_NO))
            //.ForMember(d => d.LocCode, o => o.MapFrom(s => s.LOC_CODE))
            //.ForMember(d => d.InvoiceNum, o => o.MapFrom(s => s.INVOICE_NUM))
            //.ForMember(d => d.EnergyArr, o => o.MapFrom(s => s.ENERGY_ARR))
            //.ForMember(d=>d.DbId, o=>o.MapFrom(s=>s.DB_ID))

            //.ForMember(d => d.CurrentVat, o => o.MapFrom(s => s.CURRENT_VAT))
            //.ForMember(d => d.SurchargeArr, o => o.MapFrom(s => s.SURCHARGE_ARR))
            //.ForMember(d => d.CurrentLps, o => o.MapFrom(s => s.CURRENT_LPS))
            //.ForMember(d => d.CurrDate, o => o.MapFrom(s => s.CURR_DATE))
            //.ForMember(d => d.VarArr, o => o.MapFrom(s => s.VAT_ARR))
            //.ForMember(d => d.BillCycleCode, o => o.MapFrom(s => s.BILL_CYCLE_CODE))
            //.ForMember(d => d.ConsExtgNum, o => o.MapFrom(s => s.CONS_EXTG_NUM))
            //.ForMember(d => d.MinistryName, o => o.MapFrom(s => s.MINISTRY_NAME))
            //.ForMember(d => d.MinistryNameBn, o => o.MapFrom(s => s.MINISTRY_NAMEBN))
            //.ForMember(d => d.MinistryOrder, o => o.MapFrom(s => s.MINISTRY_ORDER))
            //.ForMember(d => d.MinistryDeptName, o => o.MapFrom(s => s.MINISTRY_DEPT_NAME))
            //.ForMember(d => d.MinistryDeptNameBn, o => o.MapFrom(s => s.MINISTRY_DEPT_NAMEBN))
            //.ForMember(d => d.MinistryDeptOrder, o => o.MapFrom(s => s.MINISTRY_DEPT_ORDER))
            //.ForMember(d => d.MinistryCode, o => o.MapFrom(s => s.MINISTRY_CODE))
            //.ForMember(d => d.MinistryDeptCode, o => o.MapFrom(s => s.MINISTRY_DEPT_CODE))
            ////.ReverseMap();
            ///
            CreateMap<MinistryArrear, MinistryArrearDto>()
                .ForMember(d => d.MinistryNameBn, o => o.MapFrom(s => s.MINISTRY_NAMEBN))
                .ForMember(d => d.DeptNameBn, o => o.MapFrom(s => s.DEPT_NAMEBN))
                .ForMember(d => d.DeptCode, o => o.MapFrom(s => s.DEPT_CODE))
                .ForMember(d => d.Bill, o => o.MapFrom(s => s.BILL))
                .ForMember(d => d.HasDepartment, o => o.MapFrom(s => s.HAS_DEPARTMENT))
                .ForMember(d => d.MinistryCode, o => o.MapFrom(s => s.MINISTRY_CODE))
                .ForMember(d =>d.BillCycleCode,o=>o.MapFrom(s=>s.BILL_CYCLE_CODE))
                .ForMember(d =>d.ReceiptAmount, o => o.MapFrom(s => s.RECEIPT_AMOUNT))
                .ForMember(d =>d.TotalReceiptArrear, o => o.MapFrom(s => s.TOTAL_RECEIPT_ARREAR))
                .ReverseMap();

            CreateMap<MinistryDepartment, MinistryDepartmentDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
                .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAMEBN))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.MinistryCode, o => o.MapFrom(s => s.MINISTRY_CODE))
                .ForMember(d => d.OrderNo, o => o.MapFrom(s => s.ORDERNO))
                .ReverseMap();

            CreateMap<MinistryData, MinistryDataDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
                .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAMEBN))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
               
                .ReverseMap();

            CreateMap<MinistryDetails, MinistryDetailsDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.CUSTOMER_NAME))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.ADDRESS))
                .ForMember(d => d.CustomerNo, o => o.MapFrom(s => s.CUSTOMER_NO))
                .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
                .ForMember(d => d.LocationDsc, o => o.MapFrom(s => s.LOCATION_DSC))
                //.ForMember(d=> d.Code, o => o.MapFrom(s => s.CODE))
                //.ForMember(d=> d.Name, o=> o.MapFrom(s => s.NAME))
                .ForMember(d => d.MinistryName, o => o.MapFrom(s => s.MINISTRY_NAME))
                .ForMember(d => d.MinistryCode, o => o.MapFrom(s => s.MINISTRY_CODE))
                .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_NAME))
                .ForMember(d => d.ZoneNameBN, o => o.MapFrom(s => s.ZONE_NAME_BN))
                .ForMember(d => d.CircleName, o => o.MapFrom(s => s.CIRCLE_CODE))
                .ForMember(d => d.PrevArrearAmt, o => o.MapFrom(s => s.PREV_ARREAR_AMT))
                .ForMember(d => d.CurrMonthBill, o => o.MapFrom(s => s.CURR_MONTH_BILL))
                .ForMember(d => d.CollectionAmt, o => o.MapFrom(s => s.COLLECTION_AMOUNT))
                .ForMember(d => d.TotalArrearAmt, o => o.MapFrom(s => s.TOTAL_ARREAR_AMOUNT))
                .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
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

            CreateMap<Pouroshova, PouroshovaDTO>()
                  .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
               .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
               .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAMEBN))
               .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
               .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_NAME))
               .ForMember(d => d.CircleName, o => o.MapFrom(s => s.CIRCLE_NAME))
                .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
               .ForMember(d => d.CircleCode, o => o.MapFrom(s => s.CIRCLE_CODE))
               .ReverseMap();

            CreateMap<UnionPorishod, UnionPorishodDTO>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
              .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
              .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAMEBN))
              .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
              .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_NAME))
              .ForMember(d => d.CircleName, o => o.MapFrom(s => s.CIRCLE_NAME))
              .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
              .ForMember(d => d.CircleCode, o => o.MapFrom(s => s.CIRCLE_CODE))
              .ForMember(d => d.OrderNo, o => o.MapFrom(s => s.ORDERNO))
              .ReverseMap();

            CreateMap<MinistryArrearUpToDateMergeData, MinistryArrearUpToDateMergeDataDTO>()
              .ReverseMap();

            CreateMap<OnlineMinistryArrearDetailsMergeDTO, OnlineMinistryArearDetails>()
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

            CreateMap<MinistryDetailsSummaryMergeDTO, GetCustomerArrearModel>()
              .ForMember(d => d.ZONE_CODE, o => o.MapFrom(s => s.ZoneCode))
              .ForMember(d => d.ZONE_NAME, o => o.MapFrom(s => s.ZoneName))
              .ForMember(d => d.ZONE_NAME_BN, o => o.MapFrom(s => s.ZoneNameBn))
              .ForMember(d => d.MINISTRY_CODE, o => o.MapFrom(s => s.MinistryCode))
              .ForMember(d => d.MINISTRY_NAME_BN, o => o.MapFrom(s => s.MinistryNameBn))
              .ForMember(d => d.PRN, o => o.MapFrom(s => s.Prn))
              .ForMember(d => d.LPS, o => o.MapFrom(s => s.Lps))
              .ForMember(d => d.VAT, o => o.MapFrom(s => s.Vat))
              .ForMember(d => d.TOTAL_MINISTRY_ARREAR, o => o.MapFrom(s => s.TotalMinistryArrear))
              .ForMember(d => d.CONSUMER_NO, o => o.MapFrom(s => s.ConsumerNo))
              .ReverseMap();
        }
    }
}

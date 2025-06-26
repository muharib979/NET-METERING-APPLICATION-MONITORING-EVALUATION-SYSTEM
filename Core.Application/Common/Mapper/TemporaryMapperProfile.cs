using Core.Domain.Ministry;
using Core.Domain.Temporary;
using Shared.DTOs.Ministry;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    internal class TemporaryMapperProfile : Profile
    {
        public TemporaryMapperProfile()
        {
            CreateMap<Locations, LocationsDTO>()
                .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.LOCATION_NAME))
                .ReverseMap();

            CreateMap<MeterOwner, MeterOwnerDTO>()
                .ForMember(d => d.MeterOwnerCode, o => o.MapFrom(s => s.METER_OWNER_CODE))
                .ForMember(d => d.MeterOwnerDesc, o => o.MapFrom(s => s.METER_OWNER_DESC))
                .ReverseMap();

            CreateMap<MeterType, MeterTypeDTO>()
                .ForMember(d => d.MeterTypeDesc, o => o.MapFrom(s => s.METER_TYPE_DESC))
                .ForMember(d => d.MeterTypeCode, o => o.MapFrom(s => s.METER_TYPECODE))
                .ReverseMap();

            CreateMap<MeterCondition, MeterCondionDTO>()
               .ForMember(d => d.DefectiveCode, o => o.MapFrom(s => s.DEFECTIVE_CODE))
               .ForMember(d => d.DefectiveDesc, o => o.MapFrom(s => s.DEFECTIVE_DESC))
               .ReverseMap();

            CreateMap<Tariff, TariffDTO>()
               .ForMember(d => d.Tariff, o => o.MapFrom(s => s.TARIFF))
               .ReverseMap();

            CreateMap<BusinessType, BusinessTypeDTO>()
               .ForMember(d => d.BusTypeCode, o => o.MapFrom(s => s.BUS_TYPE_CODE))
               .ForMember(d => d.BusTypeDesc, o => o.MapFrom(s => s.BUS_TYPE_DESC))
               .ReverseMap();

            CreateMap<Feeder, FeederDTO>()
               .ForMember(d => d.FeederNo, o => o.MapFrom(s => s.FEEDER_NO))
               .ForMember(d => d.Descr, o => o.MapFrom(s => s.DESCR))
               .ReverseMap();

            CreateMap<BillGroup, BillGroupDTO>()
               .ForMember(d => d.BillGroup, o => o.MapFrom(s => s.BILL_GRP))
               .ForMember(d => d.BillGroupDesc, o => o.MapFrom(s => s.BILL_GRP_DESCR))
               .ReverseMap();

            CreateMap<BlockNum, BlockNumDTO>()                
                .ForMember(d => d.BlockNum, o => o.MapFrom(s => s.BOOK))                
                .ReverseMap();

            CreateMap<BillCycle, BillCycleDTO>()
                .ForMember(d => d.BillCycleCode, o => o.MapFrom(s => s.BILL_CYCLE_CODE))
                .ReverseMap();

            CreateMap<ScheduleMonth, ScheduleMonthDTO>()
                .ForMember(d => d.Month, o => o.MapFrom(s => s.MONTH))
                .ReverseMap();

            CreateMap<CenBillGroup, CenBillGroupDTO>()
               .ForMember(d => d.BillGroup, o => o.MapFrom(s => s.BILL_GROUP))
               .ForMember(d => d.BillGroupDesc, o => o.MapFrom(s => s.BILL_GRP_DESCR))
               .ReverseMap();
            CreateMap<MRSGenarate, MRSGenarateDTO>()
           
                .ForMember(d => d.MeterReadingId, o => o.MapFrom(s => s.METER_READING_ID))
                .ForMember(d => d.ReadingId, o => o.MapFrom(s => s.READING_ID))
                .ForMember(d => d.CustId, o => o.MapFrom(s => s.CUST_ID))
                .ForMember(d => d.CustomerNum, o => o.MapFrom(s => s.CUSTOMER_NUM))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.CUSTOMER_NAME))
                .ForMember(d => d.WalkSequence, o => o.MapFrom(s => s.WALK_SEQ))
                .ForMember(d => d.MeterNumber, o => o.MapFrom(s => s.METER_NUM))
                .ForMember(d => d.TodCode, o => o.MapFrom(s => s.TOD_CODE))
                .ForMember(d => d.TodDesc, o => o.MapFrom(s => s.TOD_DESC))
                .ForMember(d => d.TimeCycleCode, o => o.MapFrom(s => s.TIME_CYCLE_CODE))
                .ForMember(d => d.TimeCycleDesc, o => o.MapFrom(s => s.TIME_CYCLE_DESC))
                .ForMember(d => d.ReadingTypeCode, o => o.MapFrom(s => s.READING_TYPE_CODE))
                .ForMember(d => d.ReadingDescr, o => o.MapFrom(s => s.READING_DESCR))
                .ForMember(d => d.PresentReading, o => o.MapFrom(s => s.PRSNT_READING))
                .ForMember(d => d.OpenReading, o => o.MapFrom(s => s.OPN_READING))
                .ForMember(d => d.Advance, o => o.MapFrom(s => s.ADVANCE))
                .ForMember(d => d.MeterCondition, o => o.MapFrom(s => s.MTR_COND))
                .ForMember(d => d.PowerFactor, o => o.MapFrom(s => s.POWER_FACTOR))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.STATUS))
                .ReverseMap();

            CreateMap<MRSBillCalculation, MRSBillCalculationDTO>()
               .ForMember(d => d.Status, o => o.MapFrom(s => s.STATUS))
               .ForMember(d => d.ColumnName, o => o.MapFrom(s => s.COLUMN_NAME))
               .ForMember(d => d.ColumnValue, o => o.MapFrom(s => s.COLUMN_VALUE))
               .ReverseMap();

            CreateMap<MRSBillPrint,MRSBillPrintDTO>()
                .ForMember(d=>d.LocationCode,o=>o.MapFrom(s=>s.LOCATION_CODE))
                .ForMember(d => d.locationName, o => o.MapFrom(s => s.LOCATION_NAME))
                .ForMember(d => d.BillMonth, o => o.MapFrom(s => s.BILL_MONTH))
                .ForMember(d => d.BillNumber, o => o.MapFrom(s => s.BILL_NUM))
                .ForMember(d => d.BillNumberCheckDigit, o => o.MapFrom(s => s.BILL_NUM_CHK_DIGIT))
                .ForMember(d => d.CustomerNumber, o => o.MapFrom(s => s.CUSTOMER_NUM))
                .ForMember(d => d.BillIssueDate, o => o.MapFrom(s => s.BILL_ISSUE_DATE))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.CUST_NAME))
               .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.CUST_ADDRESS))
                .ForMember(d => d.Area, o => o.MapFrom(s => s.AREA))
                .ForMember(d => d.BillGroup, o => o.MapFrom(s => s.BILL_GROUP))

                .ForMember(d => d.WalkOrder, o => o.MapFrom(s => s.WALK_ORDER))
                .ForMember(d => d.PrevAcNo, o => o.MapFrom(s => s.PREV_ACC_NO))
                .ForMember(d => d.DueDate, o => o.MapFrom(s => s.DUE_DATE))
                .ForMember(d => d.Tarrif, o => o.MapFrom(s => s.TARIFF))
                .ForMember(d => d.BusCode, o => o.MapFrom(s => s.BUS_CODE))

                .ForMember(d => d.MeterNumber, o => o.MapFrom(s => s.METER_NUM))
                .ForMember(d => d.MeterType, o => o.MapFrom(s => s.METER_TYPE))
                .ForMember(d => d.MeterCondition, o => o.MapFrom(s => s.METER_COND))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.PHONE))
                .ForMember(d => d.Nid, o => o.MapFrom(s => s.NID))

                .ForMember(d => d.PowerFactor, o => o.MapFrom(s => s.POWER_FACTOR))
                .ForMember(d => d.UnitSr, o => o.MapFrom(s => s.UNIT_SR))
                .ForMember(d => d.UnitPeak, o => o.MapFrom(s => s.UNIT_PEAK))
                .ForMember(d => d.UnitOffPeak, o => o.MapFrom(s => s.UNIT_OFFPEAK))
                .ForMember(d => d.TotalUnit, o => o.MapFrom(s => s.TOTAL_UNIT))
                .ForMember(d => d.OpenUnitSr, o => o.MapFrom(s => s.OPN_UNIT_SR))
                .ForMember(d => d.OpenUnitPeak, o => o.MapFrom(s => s.OPN_UNIT_PEAK))
                .ForMember(d => d.OpenUnitOffPeak, o => o.MapFrom(s => s.OPN_UNIT_OFFPEAK))
                .ForMember(d => d.ClsUnitSr, o => o.MapFrom(s => s.CLS_UNIT_SR))
                .ForMember(d => d.ClsUnitPeak, o => o.MapFrom(s => s.CLS_UNIT_PEAK))
                .ForMember(d => d.EnergyAmountSr, o => o.MapFrom(s => s.ENERGY_AMOUNT_SR))
                .ForMember(d => d.EnergyAmountPeak, o => o.MapFrom(s => s.ENERGY_AMOUNT_PEAK))
                .ForMember(d => d.EnergyAmountOffPeak, o => o.MapFrom(s => s.ENERGY_AMOUNT_OFFPEAK))
                .ForMember(d => d.PfcAmount, o => o.MapFrom(s => s.PFC_AMOUNT))
                .ForMember(d => d.DemandCharge, o => o.MapFrom(s => s.DEMAND_CHARGE))
                .ForMember(d => d.CurrentLps, o => o.MapFrom(s => s.CURRENT_LPS))
                .ForMember(d => d.CurrentVat, o => o.MapFrom(s => s.CURRENT_VAT))
                .ForMember(d => d.CurrentPrinciple, o => o.MapFrom(s => s.CURRENT_PRN))
                .ForMember(d => d.ArrPrinciple, o => o.MapFrom(s => s.ARR_PRN))
                .ForMember(d => d.ArrVat, o => o.MapFrom(s => s.ARR_VAT))
                .ForMember(d => d.ArrLps, o => o.MapFrom(s => s.ARR_LPS))
                .ForMember(d => d.TotalPrincipleAmount, o => o.MapFrom(s => s.TOT_PRINCPAL_AMOUNT))
                .ForMember(d => d.TotalVatAmount, o => o.MapFrom(s => s.TOT_VAT_AMOUNT))
                .ForMember(d => d.TotalLpsAmount, o => o.MapFrom(s => s.TOT_LPS_AMOUNT))
                .ForMember(d => d.VatPercent, o => o.MapFrom(s => s.VAT_PERCENT))
                .ForMember(d => d.TotalBillAmount, o => o.MapFrom(s => s.TOTAL_BILL_AMOUNT))
                .ForMember(d => d.PresentDate, o => o.MapFrom(s => s.PRESENT_DATE))
                .ForMember(d => d.PresentRdg, o => o.MapFrom(s => s.PRESENT_RDG))
                .ForMember(d => d.PreviousRdg, o => o.MapFrom(s => s.PREVIOUS_RDG))
                .ReverseMap();
               





        }
    }
}

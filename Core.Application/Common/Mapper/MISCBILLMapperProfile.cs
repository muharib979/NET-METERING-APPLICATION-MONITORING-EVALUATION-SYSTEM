using Core.Domain.CustomeEntity;
using Core.Domain.MISCBILL;
using Core.Domain.Temporary;
using Core.Domain.Untracable;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class MISCBILLMapperProfile : Profile
    {
        public MISCBILLMapperProfile()
        {
            CreateMap<BillReason, BillReasonDTO>()
                .ForMember(s => s.BillReasonId, d => d.MapFrom(s => s.BILL_REASON_ID))
                .ForMember(s => s.BillReasonCode, d => d.MapFrom(s => s.BILL_REASON_CODE))
                .ForMember(s => s.BillReasonDesc, d => d.MapFrom(s => s.BILL_REASON_DESC))
                .ForMember(s => s.BillTypeCode, d => d.MapFrom(s => s.BILL_TYPE_CODE))
                .ForMember(s => s.CreateDate, d => d.MapFrom(s => s.CREATE_DATE))
                .ForMember(s => s.CreateBy, d => d.MapFrom(s => s.CREATE_BY))
                .ForMember(s => s.UpdateDate, d => d.MapFrom(s => s.UPDATE_DATE))
                .ForMember(s => s.UpdateBy, d => d.MapFrom(s => s.UPDATE_BY))
                .ForMember(s => s.Remarks, d => d.MapFrom(s => s.REMARKS))
                .ForMember(s => s.Status, d => d.MapFrom(s => s.STATUS))
                .ReverseMap();

            CreateMap<ImposedBy, ImposedByDTO>()

               .ForMember(s => s.ImposedById, d => d.MapFrom(s => s.IMPOSED_BY_ID))
               .ForMember(s => s.ImposedByCode, d => d.MapFrom(s => s.IMPOSED_BY_CODE))
               .ForMember(s => s.ImposedByDesc, d => d.MapFrom(s => s.IMPOSED_BY_DESC))
               .ForMember(s => s.CreateDate, d => d.MapFrom(s => s.CREATE_DATE))
               .ForMember(s => s.CreateBy, d => d.MapFrom(s => s.CREATE_BY))
               .ForMember(s => s.UpdateDate, d => d.MapFrom(s => s.UPDATE_DATE))
               .ForMember(s => s.UpdateBy, d => d.MapFrom(s => s.UPDATE_BY))
               .ForMember(s => s.Remarks, d => d.MapFrom(s => s.REMARKS))
               .ForMember(s => s.Status, d => d.MapFrom(s => s.STATUS))
               .ReverseMap();

            CreateMap<CustomerDetails, CustomerDetailsDTO>()

               .ForMember(s => s.CustId, d => d.MapFrom(s => s.CUST_ID))
               .ForMember(s => s.CustomerName, d => d.MapFrom(s => s.CUSTOMER_NAME))
               .ForMember(s => s.LocationCode, d => d.MapFrom(s => s.LOCATION_CODE))
               .ForMember(s => s.LocationName, d => d.MapFrom(s => s.LOCATION_NAME))
               .ForMember(s => s.TariffId, d => d.MapFrom(s => s.TARIFF_ID))
               .ForMember(s => s.TariffDesc, d => d.MapFrom(s => s.TARIFF))
               .ForMember(s => s.BusinessType, d => d.MapFrom(s => s.BUSINESS_TYPE))
               .ForMember(s => s.AreaCode, d => d.MapFrom(s => s.AREA_CODE))
               .ForMember(s => s.MeterNum, d => d.MapFrom(s => s.METER_NUM))
               .ForMember(s => s.MeterTypeDesc, d => d.MapFrom(s => s.METER_TYPE_DESC))
               .ForMember(s => s.PrvAcNo, d => d.MapFrom(s => s.PRV_AC_NO))
               .ForMember(s => s.CustomerAddr, d => d.MapFrom(s => s.CUSTOMER_ADDR))
               .ForMember(s => s.MeterCondition, d => d.MapFrom(s => s.METER_COND))
               .ForMember(s => s.MeterTypeCode, d => d.MapFrom(s => s.METER_TYPE_Code))
               .ForMember(s => s.MeterConditionCode, d => d.MapFrom(s => s.METER_CODINTION_CODE))
               .ForMember(s => s.MeterConditionDesc, d => d.MapFrom(s => s.METER_CODINTION_DESC))
               .ForMember(s => s.BusinessTypeCode, d => d.MapFrom(s => s.BUSINESS_TYPE_CODE))
               .ForMember(s=>s.LastReadingDate,d=>d.MapFrom(s => s.LAST_READING_DATE))
               .ForMember(s => s.Xformer_kva, d => d.MapFrom(s => s.XFORMER_KVA))
               .ForMember(s => s.Xformer_day_rent, d => d.MapFrom(s => s.XFORMER_DAY_RENT))
               .ForMember(s => s.LocationDeptCode, d => d.MapFrom(s => s.DEPTCODE))
               .ReverseMap();

            CreateMap<Books, BookNoDTO>()
                .ForMember(s => s.BillGroup, d => d.MapFrom(s => s.BILL_GRP))
                .ReverseMap();

            CreateMap<ScheduleYear, ScheduleYearDTO>()
                .ForMember(s => s.Year, d => d.MapFrom(s => s.YEAR))
                .ReverseMap();

            CreateMap<InitialReading, InitialReadingDTO>()
                .ForMember(s => s.TodCode, d => d.MapFrom(s => s.TOD_CODE))
                .ForMember(s => s.TodDesc, d => d.MapFrom(s => s.TOD_DESC))
                .ForMember(s => s.TimeCycleCode, d => d.MapFrom(s => s.TIME_CYCLE_CODE))
                .ForMember(s => s.TimeCycleDesc, d => d.MapFrom(s => s.TIME_CYCLE_DESC))
                .ForMember(s => s.ReadingTypeCode, d => d.MapFrom(s => s.RERADING_TYPE_CODE))
                .ForMember(s => s.ReadingTypeDesc, d => d.MapFrom(s => s.READING_TYPE_DESC))
                .ForMember(s => s.ReadingDate, d => d.MapFrom(s => s.READING_DATE))
                .ReverseMap();

            CreateMap<DcType, DcTypeDTO>()
                .ForMember(s => s.DcTypeDesc, d => d.MapFrom(s => s.DC_TYPE_DESC))
                .ForMember(s => s.DcTypeCode, d => d.MapFrom(s => s.DC_TYPE_CODE))
                .ReverseMap();

            CreateMap<VatLps, VatLpsDTO>()
                .ForMember(s => s.VatAmount, d => d.MapFrom(s => s.VAT_RATE))
                .ForMember(s => s.LpsAmount, d => d.MapFrom(s => s.LPS_RATE))
                .ReverseMap();


            //CreateMap<UntracedCustomerArrearDetailsModel, UntracebleCustArrearDetailsReportDTO>()
            //    .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_CODE))
            //    .ForMember(d => d.CircleName, o => o.MapFrom(s => s.CIRCLE_CODE))
            //    .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
            //    .ForMember(d => d.ARREAR_PRICIPAL, o => o.MapFrom(s => s.ARREAR_PRICIPAL))
            //    .ReverseMap();




        }
    }
}


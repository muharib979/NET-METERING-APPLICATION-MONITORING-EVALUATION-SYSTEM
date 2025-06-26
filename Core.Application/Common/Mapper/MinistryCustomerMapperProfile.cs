using Core.Domain.MinistryCustomer;
using Core.Domain.NonBengali;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class MinistryCustomerMapperProfile:Profile
    {
        public MinistryCustomerMapperProfile() 
        {
            //CreateMap<MinistryCustomers, MinistryCustomerDTOs>()
            //    .ForMember(d => d.CustomerNo, o => o.MapFrom(s => s.CUSTOMER_NO))
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
            //    .ForMember(d => d.Asddress, o => o.MapFrom(s => s.ADDRESS))
            //    .ForMember(d => d.MinistryCode, o => o.MapFrom(s => s.MINISTRY_CODE))
            //    .ForMember(d => d.DepartmentCode, o => o.MapFrom(s => s.DEPARTMENT_CODE))
            //    .ForMember(d => d.CitycorporationCode, o => o.MapFrom(s => s.CITYCORPORATION_CODE))
            //    .ForMember(d => d.PouroshovaCode, o => o.MapFrom(s => s.POUROSHOVA_CODE))
            //    .ForMember(d => d.UnionParishadCode, o => o.MapFrom(s => s.UNIONPARISHAD_CODE))
            //    .ForMember(d => d.ReligiousCode, o => o.MapFrom(s => s.RELIGIOUS_CODE))
            //    .ForMember(d => d.NonBengakiCampCode, o => o.MapFrom(s => s.NON_BENGALI_CAMP_CODE))
            //    .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
            //    .ForMember(d => d.CircleCode, o => o.MapFrom(s => s.CIRCLE_CODE))
            //    .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.LOCATION_CODE))
            //    .ForMember(d => d.DbCode, o => o.MapFrom(s => s.DB_CODE))
            //    .ForMember(d => d.DistrictCode, o => o.MapFrom(s => s.DISTRICT_CODE))
            //    .ForMember(d => d.DivisionCode, o => o.MapFrom(s => s.DIVISION_CODE))
            //    .ForMember(d => d.UpazilaCode, o => o.MapFrom(s => s.UPAZILA_CODE))
            //    .ReverseMap();

            //CreateMap<MinistryCustomers, MinistryCustomerGetDTOs>()
            //    .ForMember(d => d.CustomerNumber, o => o.MapFrom(s => s.CUSTOMER_NO))
            //    .ForMember(d => d.MinistryName, o => o.MapFrom(s => s.NAME))
            //    .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.ADDRESS))
            //    .ForMember(d => d.ZoneName, o => o.MapFrom(s => s.ZONE_CODE))
            //    .ForMember(d => d.CircleName, o => o.MapFrom(s => s.CIRCLE_CODE))
            //    .ForMember(d => d.CenterName, o => o.MapFrom(s => s.LOCATION_CODE))
            //    .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.DB_CODE))
            //    .ForMember(d => d.LocationDesc, o => o.MapFrom(s => s.DISTRICT_CODE))
            //    .ReverseMap();

            CreateMap<Division, DropdownResultForStringKey>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAME))
                .ReverseMap();

            CreateMap<District, DropdownResultForStringKey>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAME))
                .ReverseMap();

            CreateMap<Upozila, DropdownResultForStringKey>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAME))
                .ReverseMap();

            CreateMap<NonBengalis, DropdownResultForStringKey>()
                .ForMember(d => d.Key, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.NAMEBN))
                .ReverseMap();
        }
    }
}

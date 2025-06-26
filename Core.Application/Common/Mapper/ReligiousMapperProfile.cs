using AutoMapper;
using Core.Domain.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class ReligiousMapperProfile:Profile
    {
        public ReligiousMapperProfile() 
        {
            CreateMap<ReligiousDTOs, Religious>()
                    .ForMember(d => d.CUSTOMER_NAME, o => o.MapFrom(s => s.CustomerName))
                    .ForMember(d => d.CUSTOMER_NO, o => o.MapFrom(s => s.CustomerNo))
                    .ForMember(d => d.ADDRESS, o => o.MapFrom(s => s.Address))
                    .ForMember(d => d.CIRCLE_CODE, o => o.MapFrom(s => s.CircleCode))
                    .ForMember(d => d.CIRCLE_NAME, o => o.MapFrom(s => s.CircleName))
                    .ForMember(d => d.LOCATION_CODE, o => o.MapFrom(s => s.LocationCode))
                    .ForMember(d => d.LOCATION_NAME, o => o.MapFrom(s => s.LocationName))
                    .ForMember(d => d.ZONE_NAME, o => o.MapFrom(s => s.ZoneName))
                    .ForMember(d => d.NO_WORSHIP, o => o.MapFrom(s => s.NoWorship))
                    .ForMember(d => d.PRN, o => o.MapFrom(s => s.Prn))
                    .ForMember(d => d.VAT, o => o.MapFrom(s => s.Vat))
                    .ForMember(d => d.LPS, o => o.MapFrom(s => s.Lps))
                    .ForMember(d => d.TOTAL_ARREAR, o => o.MapFrom(s => s.TotalArrear))
                    .ReverseMap();
        } 
    }
}

using Core.Domain.ZoneCircle;
using Shared.DTOs.ZoneCircle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class ZoneCircleMapperProfile:Profile
    {
        public ZoneCircleMapperProfile()
        {
            CreateMap<Zone, ZoneDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
                .ForMember(d => d.NameBN, o => o.MapFrom(s => s.NAMEBN))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
                .ReverseMap();

            CreateMap<Circle, CircleDto>()
                .ForMember(d => d.NAME, o => o.MapFrom(s => s.NAME))
                .ForMember(d => d.NAMEBN, o => o.MapFrom(s => s.NAMEBN))
                .ForMember(d => d.CODE, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.ZONE_CODE, o => o.MapFrom(s => s.ZONE_CODE))
                .ReverseMap();

            CreateMap<Circle, CircleDTO>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.NAME))
                .ForMember(d => d.NameBn, o => o.MapFrom(s => s.NAMEBN))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.CODE))
                .ForMember(d => d.ZoneCode, o => o.MapFrom(s => s.ZONE_CODE))
                .ForMember(d => d.Temp_C, o => o.MapFrom(s => s.TEMP_C))
                .ReverseMap();
        }
    }
}

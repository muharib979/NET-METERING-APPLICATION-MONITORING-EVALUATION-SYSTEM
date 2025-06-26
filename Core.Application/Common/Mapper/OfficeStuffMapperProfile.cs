using Core.Domain.OfficeStuff;
using Shared.DTOs.OffiecStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class OfficeStuffMapperProfile : Profile
    {
        public OfficeStuffMapperProfile()
        {
            CreateMap<OfficeStuff, OfficeStuffDto>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.ID))
              .ForMember(d => d.OfficeStuffName, o => o.MapFrom(s => s.OFFICE_STUFF_NAME))
              .ForMember(d => d.Designation, o => o.MapFrom(s => s.DESIGNATION))
              .ForMember(d => d.Phone, o => o.MapFrom(s => s.PHONE))
              .ForMember(d => d.Email, o => o.MapFrom(s => s.EMAIL))
              .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IS_ACTIVE))
              .ReverseMap();
        }
    }
}
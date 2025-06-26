using Core.Domain.AppUserManagement;
using Shared.DTOs.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class AppUserMangementMapperProfile:Profile
    {
        public AppUserMangementMapperProfile()
        {
            CreateMap<AppUserManagement, AppUserManagementDTO>()
               .ForMember(d => d.UserId, O => O.MapFrom(S => S.USER_ID))
               .ForMember(d => d.UserName, O => O.MapFrom(S => S.USER_NAME))
               .ForMember(d => d.UserPassword, O => O.MapFrom(S => S.USER_PASSWORD))
               .ForMember(d => d.Email, O => O.MapFrom(S => S.EMAIL))
                .ForMember(d => d.MobileNo, O => O.MapFrom(S => S.MOBILE_NO))
                 .ForMember(d => d.UserCode, O => O.MapFrom(S => S.USER_CODE))
                 .ForMember(d => d.DesignationCode, O => O.MapFrom(S => S.DESIG_CODE))
                 .ForMember(d => d.Db, O => O.MapFrom(S => S.DB_CODE))
                 .ForMember(d => d.Location, O => O.MapFrom(S => S.LOCATION_CODE))
               .ReverseMap();

            CreateMap<AppUserDesignation, AppUserDesignationDTO>()
               .ForMember(d => d.DesignationId, O => O.MapFrom(S => S.DESIGNATION_ID))
                .ForMember(d => d.DesignationCode, O => O.MapFrom(S => S.DESIGNATION_CODE))
               .ForMember(d => d.Name, O => O.MapFrom(S => S.NAME))
               .ForMember(d => d.CreatedBy, O => O.MapFrom(S => S.CREATED_BY))
                .ForMember(d => d.CreatedDate, O => O.MapFrom(S => S.CREATED_DATE))
                .ForMember(d => d.UpdateBy, O => O.MapFrom(S => S.UPDATE_BY))
                .ForMember(d => d.UpdateDate, O => O.MapFrom(S => S.UPDATE_DATE))
               .ReverseMap();
        }

    }
}

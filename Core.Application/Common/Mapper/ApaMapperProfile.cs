using Core.Domain.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class ApaMapperProfile : Profile
    {
        public ApaMapperProfile()
        {

            CreateMap<PerformanceIndex, PerfomanceIndexDto>()
                .ForMember(d => d.Id, O => O.MapFrom(S => S.ID))
                .ForMember(d => d.Name, O => O.MapFrom(S => S.NAME))
                 .ForMember(d => d.NameBn, O => O.MapFrom(S => S.NAMEBN))
                 .ForMember(d => d.Code, O => O.MapFrom(S => S.CODE))
                 .ForMember(d => d.ProgramCode, O => O.MapFrom(S => S.PROGRAM_CODE))
                 .ForMember(d => d.IndexUnitCode, O => O.MapFrom(S => S.INDEX_UNIT_CODE))
                 .ForMember(d => d.Value, O => O.MapFrom(S => S.VALUE))
                 .ForMember(d => d.OrderBy, O => O.MapFrom(S => S.ORDERBY))
                .ReverseMap();

            CreateMap<Program, ProgramDTO>()
                .ForMember(d => d.Id, O => O.MapFrom(S => S.ID))
                .ForMember(d => d.Name, O => O.MapFrom(S => S.NAME))
                 .ForMember(d => d.NameBn, O => O.MapFrom(S => S.NAMEBN))
                 .ForMember(d => d.Code, O => O.MapFrom(S => S.CODE))
                 .ForMember(d => d.ObjectiveCode, O => O.MapFrom(S => S.OBJECTIVE_CODE))
                 .ForMember(d => d.OrderBy, O => O.MapFrom(S => S.ORDERBY))
                .ReverseMap();

            CreateMap<Target, TargetDTO>()
               .ForMember(d => d.Id, O => O.MapFrom(S => S.ID))
               .ForMember(d => d.FiscalYearCode, O => O.MapFrom(S => S.FISCALYEAR_CODE))
                .ForMember(d => d.PerformanceIndexCode, O => O.MapFrom(S => S.PERFORMANCE_INDEX_CODE))
                .ForMember(d => d.Value, O => O.MapFrom(S => S.VALUE))
               .ReverseMap();

            CreateMap<FinancialYear, FinancialYearDTO>()
              .ForMember(d => d.Id, O => O.MapFrom(S => S.ID))
              .ForMember(d => d.FinancialName, O => O.MapFrom(S => S.FINANCIAL_NAME))
               .ForMember(d => d.StartMonth, O => O.MapFrom(S => S.START_MONTH))
                .ForMember(d => d.EndMonth, O => O.MapFrom(S => S.END_MONTH))
               .ForMember(d => d.Code, O => O.MapFrom(S => S.CODE))
              .ReverseMap();

        }
    }
}

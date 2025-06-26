using Core.Domain.Reconciliation;
using Shared.DTOs.Reconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Common.Mapper
{
    public class ReconciliationMapperProfile : Profile
    {
        
        public ReconciliationMapperProfile()
        {

            CreateMap<ReconcilationStatus, ReconcilationStatusDTO>()
                .ForMember(d => d.PayDate, O => O.MapFrom(S => S.PAY_DATE))
                .ForMember(d => d.NoOfTransaction, O => O.MapFrom(S => S.NO_OF_TRANSACTION))
                 .ForMember(d => d.TotalAmount, O => O.MapFrom(S => S.TOTAL_AMOUNT))
                 .ForMember(d => d.PrincipleAmount, O => O.MapFrom(S => S.PRINCIPAL_AMOUNT))
                 .ForMember(d => d.VatAmount, O => O.MapFrom(S => S.VAT_AMOUNT))
                  .ForMember(d => d.User, O => O.MapFrom(S => S.UPDATE_BY))
                .ReverseMap();
        }
    }
}

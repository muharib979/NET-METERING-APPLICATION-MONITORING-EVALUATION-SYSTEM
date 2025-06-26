using Core.Application.Interfaces.MiscBilling;
using Core.Domain.Temporary;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class SaveBillCycleCommand : BillScheduleDTO, IRequest<int>
    {
        public class Handler : IRequestHandler<SaveBillCycleCommand, int>
        {
            
            private readonly IScheduleRepository _repository;

         

            public Handler(IScheduleRepository repository)
            {
                _repository= repository;
            }

            public async Task<int> Handle(SaveBillCycleCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveBillSchedule(request);
                return result;

            }
        }
    }
}

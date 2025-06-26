using Core.Application.Interfaces.PaymentGateway;
using Shared.DTOs.PaymentGatewayDto;
using Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.PaymentGateway
{
    public class SaveBkashPaymentFromAppCommand:IRequest<BkashCallBackResponseDTO>
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string customerNo { get; set; }
        public string? billMonth { get; set; }
        public string amount { get; set; }
        public string? mobileNumber { get; set; }
        public string? transactionId { get; set; }
        public string? payTime { get; set; }

        public class Handler : IRequestHandler<SaveBkashPaymentFromAppCommand, BkashCallBackResponseDTO>
        {
            private readonly IBkashAppRepository _appRepository;

            public Handler(IBkashAppRepository appRepository)
            {
                _appRepository = appRepository;
            }

            public async Task<BkashCallBackResponseDTO> Handle(SaveBkashPaymentFromAppCommand command,CancellationToken token)
            {
                try
                {
                    var result = await _appRepository.SaveBkashPaymentFromApp
                        (command.userName, command.password, command.customerNo, command.billMonth, command.amount, command.mobileNumber, command.transactionId, command.payTime);
                    return result;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }
    }
}

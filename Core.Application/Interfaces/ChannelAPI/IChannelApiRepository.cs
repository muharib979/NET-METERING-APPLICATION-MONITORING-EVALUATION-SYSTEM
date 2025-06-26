using Microsoft.AspNetCore.Http;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ChannelAPI
{
    public interface IChannelApiRepository
    {
        Task<(bool IsSaved, string messages)> TransferPrePaidServer(PostpaidCustDetailsDTO model);
       
        Task<Result> GetPrepaidMODData(PrePaidToPostPaidMOD model,string locationCode);
        Task<Result> GetCustomerTranferList(PrePaidToPostPaidMOD model);
        Task<CustomerInformation> GetPrepaidCustomerByTrans(PrePaidToPostPaidDTO model);
    }
}

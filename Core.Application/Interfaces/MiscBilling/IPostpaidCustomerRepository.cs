using Microsoft.AspNetCore.Http;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.Response;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IPostpaidCustomerRepository
    {
        Task<PostpaidCustDetailsDTO> GetPostpaidCustomerByNumber(string custNumber, string locationCode);
        Task<(bool IsSaved, string messages)> SavePostpaidToPrepaid(PostpaidCustDetailsDTO model);
        Task<List<PostpaidCustDetailsDTO>> GetPrepaidList(string locationCode);
        Task<List<PostpaidCustDetailsDTO>> PostPaidToPrepaidPrint(string custNumber,string locationCode);
        Task<List<PostToPrepaidViewDTO>> GetPostToPrepaidByDate(string startDate, string endDate, string locationCode);
        Task<List<PostpaidCustDetailsDTO>> GetPostToPrepaidSearchByDate(string startDate, string endDate, string locationCode);
        Task<Result> GetPostToPrepaidForUpdate(string startDate, string endDate, string locationCode);
        Task<CustomerInformation> GetPrepaidCustomerByTransId(string transID);
        Task<int> UpdatePostToPrepaidCustomer(List<PostpaidCustDetailsDTO> model);
        Task<Result> GetPrepaidMODDATA(PrePaidToPostPaidMOD model);
        Task<List<CustomerDetailsDTOByBookNumber>> GetPostPaidCustomerByBookNumber(string bookNumber, string locationCode);
        Task<List<BillGroupDTO>> GetAllBillGroup(string locationCode);
        Task<List<BlockNumDTO>> GetAllBookNumber(string locationCode, string billgroup);
        Task<List<DivisionDTO>> GetDivisionForPrepaidCustomer();
        Task<List<DistrictDTO>> GetDistrictForPrepaidCustomer();
        Task<List<ThanaDTO>> GetThanaForPrepaidCustomer();
        Task<string> PrepaidToPostPaid(PrepaidToPostPaidTransferDTO model);
        Task<string> FDMToPostpaidSave(PostpaidCustFDMDTO model);
        Task<string> KWHToPostpaidSave(PostpaidCustFDMDTO model);
        Task<List<ModPrepaidCustomerDTO>>GetModPrepaidCustomerr(string zoneCode, string locationCode,string fromDate, string toDate);
        Task<List<LocationWiseCustomerDTO>> LocationWiseCustomerCount(string zoneCode, string locationCode);
    }
}

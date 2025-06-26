using Core.Domain.Ministry;
using Shared.DTOs.Ministry;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MinistryCustomer
{
    public interface IMinistryCustomerRepository
    {
        Task<List<DropdownResultForStringKey>> GetAllPouroshova();
        Task<List<DropdownResultForStringKey>> GetAllUnionParishod();
        Task<List<DropdownResultForStringKey>> GetAllMinistryDepartment(string ministryCode);
        Task<MinistryDataViewDTO> SaveMinistryCustomer(MinistryCustomerDTOs model);
        Task<List<MinistryCustomerGetDTOs>> GetAllMinistryCustomer(string? customerNo, string? centerCode, string? locationCode, string? ministryCode, string? cityCode, bool? isRebate);
        Task<MinistryCustomerDTOs> GetMinistryCustomerById(string customerNo, string locationCode);
        
        Task<MinistryCustomerDTOs> GetMinistryCustomerByCustomerNo(string customerNo);
        Task<int> DeleteMinistryCustomer(int customerNo);
        Task<List<DropdownResultForStringKey>> GetAllDivision();
        Task<List<DropdownResultForStringKey>> GetAllDistrict(string divisionCode);
        Task<List<DropdownResultForStringKey>> GetAllUpozila(string districtCode);
        Task<List<DropdownResultForStringKey>> GetAllNonBengaliCamp();
        Task<MinistryDataViewDTO> GetCustomerData(string customerNo, string locationCode);

        Task<List<MinistryWiseCustomerCountDTO>> GetMinistryWiseCustomerCount(string ministryCode, string locationCode);

       
    }
}

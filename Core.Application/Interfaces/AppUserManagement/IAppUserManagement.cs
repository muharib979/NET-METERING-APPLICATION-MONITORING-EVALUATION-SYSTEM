using Shared.DTOs.AppUserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.AppUserManagement
{
    public interface IAppUserManagement
    {
        Task<int> SaveAppUserManagementBill(AppUserManagementDTO model);
        Task<List<AppUserManagementDTO>> GetAppUserManagementList();

        Task<List<AppUserDesignationDTO>> GetAppUserDesignationList();
        Task<int> DeleteAppMangementBill(int id);
    }
}

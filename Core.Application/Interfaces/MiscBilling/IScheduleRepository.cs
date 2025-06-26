using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IScheduleRepository
    {
        Task<List<CenBillGroupDTO>> GetScheduleBillGroup();
        Task<List<ScheduleYearDTO>> GetScheduleYear();
        Task<List<ScheduleMonthDTO>> GetScheduleMonth(string year);
        Task<int> SaveBillSchedule(BillScheduleDTO model);
    }
}

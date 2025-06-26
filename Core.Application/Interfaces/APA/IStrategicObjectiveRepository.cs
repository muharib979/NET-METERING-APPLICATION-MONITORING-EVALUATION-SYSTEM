using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface IStrategicObjectiveRepository
    {
        Task<List<StrategicObjectiveDto>> GetAllStrategicObjectiveList();
        Task<bool> UpdateStrategicObjectiveBill(StrategicObjectiveDto model);
        Task<bool> SaveStrategicObjectiveBill(StrategicObjectiveDto model);
        Task<int> DeleteStrategicObjectiveBill(int id);
    }
}

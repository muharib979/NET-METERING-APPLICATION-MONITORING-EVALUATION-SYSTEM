using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface IProgramRepository
    {
        Task<List<ProgramDTO>> GetAllProgramDataByStrategicCode(string strategicCode);
        Task<List<ProgramDTO>> GetAllProgramList();
        Task<bool> SaveProgramBill(ProgramDTO model);
        Task<int> DeleteProgramBill(int id);
    }
}

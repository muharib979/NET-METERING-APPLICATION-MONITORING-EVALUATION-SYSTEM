using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.APA
{
    public interface IIndexUnitRepository
    {
        Task<List<IndexUnitDto>> GetIndexUnitDataList();
        Task<bool> SaveIndexUnitBill(IndexUnitDto model);
        Task<int> DeleteIndexUnitBill(int id);
    }
}

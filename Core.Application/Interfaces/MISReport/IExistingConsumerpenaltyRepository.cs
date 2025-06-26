using Shared.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace Core.Application.Interfaces.MISReport
{
    public interface IExistingConsumerpenaltyRepository
    {
       Task<List<ExistingConsumerPenaltyDTO>>GetExistingConsumerPenalty(string zoneCode, string billMonth);
      
    }
}

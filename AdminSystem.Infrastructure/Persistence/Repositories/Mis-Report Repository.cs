using Core.Application.Interfaces.MISReport;
using Shared.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories
{
    public class Mis_Report_Repository : IExistingConsumerpenaltyRepository
    {
        public Task<List<ExistingConsumerPenaltyDTO>> GetExistingConsumerPenalty(string zoneCode, string billMonth)
        {
            throw new NotImplementedException();
        }
    }
}

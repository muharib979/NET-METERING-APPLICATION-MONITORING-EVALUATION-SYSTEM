using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.MiscBilling;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.MiscBilling
{
    public class SupplementaryBillPrintRepository
    {
        private readonly IMapper _mapper;

        public SupplementaryBillPrintRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

      
    }
}

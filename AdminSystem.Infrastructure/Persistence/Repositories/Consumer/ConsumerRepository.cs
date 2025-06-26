using AdminSystem.Infrastructure.Persistence.Context;
using AutoMapper;
using Core.Application.Interfaces.Consumer;
using Core.Domain.Nem;
using Oracle.ManagedDataAccess.Client;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminSystem.Infrastructure.Persistence.Repositories.Consumer
{
    public class ConsumerRepository : IConsumerRepository
    {
        private readonly IMapper _mapper;
        public async Task<ConsumerDto> GetConsumerDetails(string accountNumber)
        {
           ConsumerDto consumer = new ConsumerDto();
            using var con = new OracleConnection(Connection.ConnectionString());
            string sQuery = @"SELECT ACCOUNTNUMBER AccountNumber,NAME Name, METERNUMBER MeterNumber,LOAD Load,TARIFFNAME Tariff,OFFICECODE OfficeCode,VOLTAGELEVELNEM VoltageLevel,SITEADDRESS SiteAddress FROM se WHERE ACCOUNTNUMBER =:accountNumber";
             consumer = await con.QueryFirstOrDefaultAsync<ConsumerDto>(sQuery, new { AccountNumber = accountNumber });
            return consumer;
        }
    }
}

using Core.Domain.Nem;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Consumer
{
    public interface IConsumerRepository
    {

        Task<ConsumerDto> GetConsumerDetails(string accountNumber);
    }
}

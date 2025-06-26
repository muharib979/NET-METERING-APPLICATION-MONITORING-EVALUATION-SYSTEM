using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Location.GetLocationById
{
    public class GetLocationByIdQuery : IRequest<Response<LocationsDto>>
    {
        public int Id { get; set; }
    }
}

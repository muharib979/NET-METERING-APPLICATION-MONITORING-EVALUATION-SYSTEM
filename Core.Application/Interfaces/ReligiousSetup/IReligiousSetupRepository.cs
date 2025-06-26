using Shared.DTOs.ReligiousSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.ReligiousSetup
{
    public interface IReligiousSetupRepository
    {
        Task<bool> SetupReligious(ReligiousSetupDTO model);
    }
}

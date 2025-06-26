using Shared.DTOs.VisitorDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.VisitorDetails
{
    public interface IVisitorDetails
    {
        Task<bool> SaveVisitorDetails(string userName);
        Task<List<VisitorCountDTO>> GetVisitorCount();

        //Task<TokenResponseDtos> NemCheckValidate(TokenRequestDto requestDto);
    }
}

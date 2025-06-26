using Shared.DTOs.MISCBILL;
using Shared.DTOs.Temporary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IBookBillGroupRepository
    {
        Task<List<BookNoDTO>> GetBookNoAsync(string bookno, string locationcode, string user);
        Task<List<BillGroupDTO>> GetAllBillGroup(string locationCode, string bookNumber);
    }
}

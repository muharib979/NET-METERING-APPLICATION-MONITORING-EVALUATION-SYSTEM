using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IBillDetailsRepository<T> where T : class
    {
        Task<List<T>> BillDetailsByCustomer(string customerNumber, string locationCode);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.MiscBilling
{
    public interface IBillPrintRepository<T>where T:class
    {
        Task<List<T>> PenaltyBillSrPrint(string customerNumber, string billNumber);
        Task<List<T>> PenaltyBillNonCustPrint(string customerNumber, string billNumber);
        Task<List<T>> PenaltyBillPrepaidSrPrint(string customerNumber, string billNumber);
    }
}

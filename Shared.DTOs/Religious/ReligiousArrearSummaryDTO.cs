using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Religious
{
    public class ReligiousArrearSummaryDTO
    {
        public string FinancialYear { get; set; }
        public int ConsumerCount { get; set; }
        public int RebaitUnit { get; set; }
        public decimal TariffRate { get; set; }
        public int CountMonth { get; set; }
        public decimal Principal { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal DemandCharge { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalArrear { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
    }
}

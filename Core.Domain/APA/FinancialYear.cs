using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.APA
{
    public class FinancialYear
    {
        public int? ID { get; set; }
        public string FINANCIAL_NAME { get; set; }
        public string? START_MONTH { get; set; }
        public string? END_MONTH { get; set; }
        public string? CODE { get; set; }
    }
}

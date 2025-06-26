using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Religious
{
    public class Religious
    {
        public string CUSTOMER_NAME { get; set; }
        public int CUSTOMER_NO { get; set; }
        public string ADDRESS { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string ZONE_NAME { get; set; }

        public int NO_WORSHIP { get; set; }
        public decimal PRN { get; set; }
        public decimal VAT { get; set; }
        public decimal LPS { get; set; }
        public decimal TOTAL_ARREAR { get; set; }
    }
}

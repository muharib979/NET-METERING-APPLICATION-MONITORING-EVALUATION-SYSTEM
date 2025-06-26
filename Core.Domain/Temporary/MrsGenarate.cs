using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Temporary
{
    public class MRSGenarate
    {
       
        public int? METER_READING_ID { get; set; }
        public int? READING_ID { get; set; }
        public int? CUST_ID { get; set; }
        public string? CUSTOMER_NUM { get; set; }
        public string? CUSTOMER_NAME { get; set; }
        public string? WALK_SEQ { get; set; }
        public string? METER_NUM { get; set; }
        public string? TOD_CODE { get; set; }
        public string? TOD_DESC { get; set; }
        public string? TIME_CYCLE_CODE { get; set; }
        public string? TIME_CYCLE_DESC { get; set; }
        public string? READING_TYPE_CODE { get; set; }
        public string? READING_DESCR { get; set; }
        public string? PRSNT_READING { get; set; }
        public string? OPN_READING { get; set; }
        public string? ADVANCE { get; set; }
        public string? MTR_COND { get; set; }
        public string? POWER_FACTOR { get; set; }
        public int? STATUS { get; set; }

    }
}

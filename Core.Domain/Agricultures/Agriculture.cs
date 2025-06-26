using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Agricultures
{
    public class Agriculture
    {
        //public int ID { get; set; }
        //public int CUST_ID { get; set; }
        //public string? LOCATION_CODE { get; set; }
        //public string? CUSTOMER_NAMEBN { get; set; }
        //public string? AGRICULTURE_LOCATIONBN { get; set; }
        //public string? NAMEBN { get; set; }
        //public string? CON_EXTG_NUM { get; set; }
        //public string? PROVIDER_NAME { get; set; }
        //public DateTime CURR_DATE { get; set; }
        //public int CONSUMER_NO { get; set; }
        //public decimal ARREAR_AMT { get; set; }

        public int CUSTOMER_NO { get; set; }
        public string CUSTOMER_NAMEBN { get; set; }
        public string DEPT_NAMEBN { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_DESCBN { get; set; }
        public int IS_KRISHI { get; set; }
        public int IS_POULTRY { get; set; }
        public string CON_EXTG_NUM { get; set; }
        public int ARREAR_AMT { get; set; }
    }
}

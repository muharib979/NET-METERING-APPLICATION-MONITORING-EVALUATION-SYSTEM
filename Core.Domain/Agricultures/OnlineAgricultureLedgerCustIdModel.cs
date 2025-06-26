using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Agricultures
{
    public class OnlineAgricultureLedgerCustIdModel
    {
        public int CUST_ID { get; set; }
        public string KRISHI_DEPARTMRNT_CODE { get; set; }
        public string KRISHI_DEPT_NAME_BN { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME_BN { get; set; }
        public int IS_KRISHI { get; set; }
        public int IS_POULTRY { get; set; }
        public string CON_EXTG_NUM { get; set; }
        public string LOCATION_NAME { get; set; }
        public string FINANCIAL_YEAR { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ministry
{
    public class PublicSecurityDivisionCustId
    {
        public int CUST_ID { get; set; }
        public string ZONE_CODE { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string MINISTRY_NAME { get; set; }
        public string MINISTRY_NAME_BN { get; set; }
        public string ZONE_NAME { get; set; }
        public string ZONE_NAME_BN { get; set; }
        public int HAS_DEPARTMENT { get; set; }
        public int ORDERNO { get; set; }
        public string DEPT_NAME { get; set; }
    }
}

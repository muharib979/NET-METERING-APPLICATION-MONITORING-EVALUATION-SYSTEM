using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Ministry
{
    public class MinistryArrear
    {
        
        public string MINISTRY_NAMEBN { get; set; }
        public string MINISTRY_CODE { get; set; }
        public string DEPT_NAMEBN { get; set; }
        public string DEPT_CODE { get;}
        public decimal BILL { get; set; }
        public int HAS_DEPARTMENT { get; set; }
        public string BILL_CYCLE_CODE { get; set; }
        public decimal RECEIPT_AMOUNT { get; set; }
        public decimal TOTAL_RECEIPT_ARREAR { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MinistryCustomer
{
    public class District
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public int ORDERNO { get; set; }
        public string DIVISION_CODE { get; set; }
        public int ISACTIVE { get; set; }
    }
}

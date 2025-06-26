using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Domain.MinistryCustomer
{
    public class Division
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public int ORDERNO { get; set; }
        public int IS_ACTIVE { get; set; }
        public string CODE { get; set; }
    }
}

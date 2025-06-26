using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.CustomeEntity
{
    public class CustomerTariff
    {
        public int TARIFF_ID { get; set; }
        public string TARIFF_CODE { get; set; } 
        public string TARIFF_DESC { get; set; }
        public string STATUS { get; set; }
        public DateTime CREATE_DATE { get; set; }
    }
}

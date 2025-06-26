using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.CustomeEntity
{
    public class CustomerType
    {
        public int CUST_TYPE_ID { get; set; }
        public string? CUST_TYPE_CODE { get; set; }
        public string? CUST_TYPE_DESC { get; set; }
        public string? REMARKS { get; set; }
        public string? STATUS { get; set; }
        public string? CREATE_BY { get; set; }
        public DateTime? CREATE_DATE { get; set; }
        public string? UPDATE_BY { get; set; }
        public DateTime? UPDATE_DATE { get; set; }
    }
}

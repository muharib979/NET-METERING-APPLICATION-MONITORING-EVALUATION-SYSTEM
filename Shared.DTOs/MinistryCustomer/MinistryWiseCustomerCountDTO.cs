using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MinistryCustomer
{
    public class MinistryWiseCustomerCountDTO
    {
        public string MinistryName { get; set; }
        public string MinistryCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int TotalCustomer { get; set; }
        public int CityCorporationCount { get; set; }
        public int PouroshovaCount { get; set; }
        public int UnionPorishodCount { get; set; }
        public int RebaitCount { get; set; }
    }
}

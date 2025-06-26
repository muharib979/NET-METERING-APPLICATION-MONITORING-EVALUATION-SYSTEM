using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class UserInfoDto
    {
        public string CustomerNumber { get; set; }
        public string LocationCode { get; set; }
        public string Month { get; set; }
        public decimal MonthArr { get; set; }
    }
}

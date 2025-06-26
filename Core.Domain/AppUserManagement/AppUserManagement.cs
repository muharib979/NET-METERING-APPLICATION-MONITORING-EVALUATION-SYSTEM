using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.AppUserManagement
{
    public class AppUserManagement
    {
        public int? USER_ID { get; set; }
        public string? USER_NAME { get; set; }
        public string? USER_PASSWORD { get; set; }
        public string? EMAIL { get; set; }
        public string? MOBILE_NO { get; set; }
        public string? USER_CODE { get; set; }
        public string? DESIG_CODE { get; set; }
        public string? DB_CODE { get; set; }
        public string? LOCATION_CODE { get; set; }
    }
}

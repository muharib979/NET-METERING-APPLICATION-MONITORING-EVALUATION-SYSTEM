using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.AppUserManagement
{
    public class AppUserDesignation
    {
        public int? DESIGNATION_ID { get; set; }

        public string? NAME { get; set; }
        public string? DESIGNATION_CODE { get; set; }
        public string? CREATED_BY { get; set; }

        public string? CREATED_DATE { get; set; }
        public  string? UPDATE_BY { get; set; }
        public string? UPDATE_DATE { get; set; }
    }
}

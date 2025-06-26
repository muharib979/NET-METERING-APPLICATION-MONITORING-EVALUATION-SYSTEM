using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ProsoftDataSync
{
    public class Employee
    {
        public Guid col_id { get; set; }
        public string col_employee_id { get; set; }
        public string? col_rank { get; set; }
        public string col_svc_no { get; set; }
        public string? col_name { get; set; }
        //public DateTime? col_date { get; set; }
        public string? col_clock_type { get; set; }
        public DateTime? col_clock_time { get; set; }
        public string? col_site_id { get; set; }
        public string? col_shift_code { get; set; }

        public string? col_deployment { get; set; }
        public DateTime col_entry_time { get; set; }
    }
}

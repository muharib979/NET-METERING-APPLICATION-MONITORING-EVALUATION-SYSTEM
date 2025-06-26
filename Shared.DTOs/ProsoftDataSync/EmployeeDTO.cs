using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ProsoftDataSync
{
    public class EmployeeDTO
    {
        public string? EmployeeId { get; set; }
        public string? Rank { get; set; }
        public string? SvcNo { get; set; }
        public string? Name { get; set; }
        //public DateTime? Date { get; set; }
        public string Date { get; set; }
        public string? ClockType { get; set; }
        //public DateTime? Clock_Time { get; set; }
        public string ClockTime { get; set; }
        public string? SiteId { get; set; }
        public string? ShiftCode { get; set; }

        public string? Deployment { get; set; }


    }
}

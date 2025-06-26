using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Temporary
{
    public class BillScheduleDTO
    {
        public string? LocationCode { get; set; }
        public string? BillGroup { get; set; }
        public string? BillYear { get; set; }
        public string? BillMonth { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? ReadingDate { get; set; }
        public string? DueDate { get; set; }
        public string? UserName { get; set; }
    }

    public class ScheduleYearDTO
    {
        public string? Year { get; set; }
    }

    public class ScheduleMonthDTO
    {
        public string? Month { get; set; }
    }
}

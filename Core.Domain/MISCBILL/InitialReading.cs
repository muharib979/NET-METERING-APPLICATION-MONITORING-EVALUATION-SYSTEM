using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.MISCBILL
{
    public class InitialReading
    {
        public string? TOD_CODE { get; set; }
        public string? TOD_DESC { get; set; }
        public string? TIME_CYCLE_CODE { get; set; }
        public string? TIME_CYCLE_DESC { get; set; }
        public string? RERADING_TYPE_CODE { get; set; }
        public string? READING_TYPE_DESC { get; set; }
        public string? READING_DATE { get; set; }
    }
}

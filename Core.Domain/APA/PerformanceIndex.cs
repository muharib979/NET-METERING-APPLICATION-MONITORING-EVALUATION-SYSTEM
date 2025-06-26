using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.APA
{
    public class PerformanceIndex
    {
        public int? ID { get; set; }
        public string? NAME { get; set; }
        public string? NAMEBN { get; set; }
        public string? CODE { get; set; }
        public string? PROGRAM_CODE { get; set; }
        public string? INDEX_UNIT_CODE { get; set; }
        public int? VALUE { get; set; }
        public int? ORDERBY { get; set; }
    }
}

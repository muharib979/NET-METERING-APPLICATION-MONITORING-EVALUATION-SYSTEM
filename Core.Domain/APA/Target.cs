using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.APA
{
    public class Target
    {
        public  int? ID { get; set; }
        public string? FISCALYEAR_CODE { get; set; }
        public string? PERFORMANCE_INDEX_CODE { get; set; }
        public int? VALUE { get; set; }
    }
}

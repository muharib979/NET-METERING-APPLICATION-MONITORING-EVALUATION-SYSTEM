using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class PerfomanceIndexDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameBn { get; set; }
        public string? Code { get; set; }
        public string? ProgramCode { get; set; }
        public string? PoName { get; set; }
        public string? IndexUnitCode { get; set; }
        public string? IuName { get; set; }
        public int? Value { get; set; }
        public int? OrderBy { get; set; }
    }
}

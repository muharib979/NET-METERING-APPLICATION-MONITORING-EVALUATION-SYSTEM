using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.VisitorDetails
{
    public class VisitorCountDTO
    {
        public string? UserName { get; set; }
        public string? IpAddress { get; set; }
        public string? LastLogin { get; set; }
        public int? VisitorCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class PouroshovaDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameBn{ get; set; }
        public string? Code { get; set; }
        public string? ZoneName { get; set; }
        public string? ZoneCode { get; set; }
        public string? CircleName { get; set; }
        public string? CircleCode { get; set; }
    }
}

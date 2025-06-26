using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ZoneCircle
{
    public class CircleDto
    {
        public int ID { get; set; }
        public string? NAME { get; set; }
        public string? NAMEBN { get; set; }
        public string? CODE { get; set; }
        public string? ZONE_CODE { get; set; }
    }
    public class CircleDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameBn { get; set; }
        public string? Code { get; set; }
        public string? ZoneCode { get; set; }
        public string? Temp_C { get; set; }
    }
}

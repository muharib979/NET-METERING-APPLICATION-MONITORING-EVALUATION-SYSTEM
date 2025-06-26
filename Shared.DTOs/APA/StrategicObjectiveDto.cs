using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.APA
{
    public class StrategicObjectiveDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NameBn { get; set; }
        public string? Code { get; set; }
        public int? OrderBy { get; set; }
    }
    public class ReturnDTO
        {
        public int O_Status { get; set; }
    }

       
}

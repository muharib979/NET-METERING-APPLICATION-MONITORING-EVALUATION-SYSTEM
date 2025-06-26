using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class ViewMinistryDTO
    {
        public string? Name { get; set; }
        public string? NameBn { get; set; }
        public string? Code { get; set; }
        public int? HasDepartment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Ministry
{
    public class MinistryDepartmentDto
    {
        public string Name { get; set; }
        public string NameBn { get; set; }
        public string Code { get; set; }
        public string MinistryCode { get; set; }
        public int OrderNo { get; }
    }
}

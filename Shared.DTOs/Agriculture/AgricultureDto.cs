using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Agriculture
{
    public class AgricultureDto
    {
        public int CustomerNo { get; set; }
        public string CustomerNameBn { get; set; }
        public string DeptNameBn { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescBn { get; set; }
        public int IsKrishi { get; set; }
        public int IsPoultry { get; set; }
        public string ConExtgNum { get; set; }
        public int ArrearAmt { get; set; }
    }
}

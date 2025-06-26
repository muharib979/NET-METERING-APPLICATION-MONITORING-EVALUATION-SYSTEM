using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISReport
{
    public class CenterWiseArrearSummaryDTO
    {
        public string Center { get; set; }
        public int Noc { get; set; }
        public string Month { get; set; }
        public int BldUnit { get; set; }
        public decimal CurPrin { get; set; }
        public decimal CurVat { get; set; }
        public decimal ArrPrin { get; set; }
        public decimal ArrVat { get; set; }
        public decimal ArrLps { get; set; }
        public decimal TotalBill { get; set; }
        public decimal CurrentLps { get; set; }
        public int Order { get; set; }
        public string Loc { get; set; }
        public string Office { get; set; }
    }
}

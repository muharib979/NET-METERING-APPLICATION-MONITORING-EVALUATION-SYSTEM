using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class InstallmentPlanDTO
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string? Remarks { get; set; }
        public string DueDate { get; set; }
        public int Check { get; set; }
        public int InstallNumber { get; set; }
        public decimal InstallmentPercn { get; set; }
        public decimal LpsAmount { get; set; }
        public decimal PrincipleAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? TotalInAmount { get; set; }
        public string? UserName { get; set; }
        public string? Location { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? Tariff { get; set; }
        public string? BillAmount { get; set; }

    }

    public class ReturnInstallMentValue
    {
        public int? InstallmentId { get; set; }
        public int? Status { get; set; }
        public int? O_Status { get; set; }

    }
}

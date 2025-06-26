using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class SupplementaryExistingConsumerDTO
    {
        public int? BILL_ID { get; set; }
        public string? BILL_NO { get; set; }
        public string? LocationCode { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddr { get; set; }
        public string? TariffDesc { get; set; }
        public string? BbusinessType { get; set; }
        public string? PrvAcNo { get; set; }
        public string? AreaCode { get; set; }
        public string? NidNumber { get; set; }
        public string? MobileNumber { get; set; }

        public string? MeterTypeDesc { get; set; }
        public string? MeterNum { get; set; }
        public string? MeterConditionDesc { get; set; }
        public string? MeterTypeCode { get; set; }
        public string? LastReadingOffPeak { get; set; }
        public string? LastReadingPeak { get; set; }
        public string? LDate { get; set; }

        public string? BillReasonCode { get; set; }
        public decimal? PrincipleAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public string? ImposedByCode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? DDate { get; set; }
        public string? CreateBy { get; set; }
        public int? CustId { get; set; }
        public string? LastReadingDate { get; set; }
        public string? DueDate { get; set; }
        public int? FormType { get; set; }
    }
}

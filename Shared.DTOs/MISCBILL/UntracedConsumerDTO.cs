using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.MISCBILL
{
    public class UntracedConsumerDTO
    {
        public int? Id { get; set; }
        public int? CustId { get; set; }
        public string? CustomerNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? CustomerAddr { get; set; }
        public string? BusinessType { get; set; }
        public int? TariffId { get; set; }
        public string? TariffDesc { get; set; }
        public string? AreaCode { get; set; }
        public string? PrvAcNo { get; set; }
        public string? MeterNum { get; set; }
        public string? MeterTypeDesc { get; set; }
        public string? MeterConditionDesc { get; set; }
        public int? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public  string? UcType { get; set; }
    }
}

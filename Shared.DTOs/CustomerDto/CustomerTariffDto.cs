using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.CustomerDto
{
    public class CustomerTariffDto
    {
        public int TariffId { get; set; }
        public string TariffCode { get; set; }
        public string TariffDesc { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

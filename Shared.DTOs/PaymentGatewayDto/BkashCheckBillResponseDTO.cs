using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentGatewayDto;

public class BkashCheckBillResponseDTO
{
    public string BillMonth { get; set; }
    public string BillAmount { get; set; }
    public string BillDueDate { get; set; }
    public string QueryTime { get; set; }
    public AmountBreakDownDTO AmountBreakDown { get; set; }
}
public class AmountBreakDownDTO
{
    public string EnergyCost { get; set; }
    public string VatAmount { get; set; }
}

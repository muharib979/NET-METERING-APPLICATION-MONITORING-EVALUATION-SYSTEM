using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentGatewayDto;

public class BkashCallBackResponseDTO
{
    public string ConsumerName { get; set; }
    public string TotalAmount { get; set; }
    public string TrxId { get; set; }
    public string MiddlewarePayTime { get; set; }
    public string RefNumber { get; set; }
    public string CustomMessage { get; set; }
    public AmountBreakDownDTO AmountBreakDown { get; set; }
}

public class BkashBillImageDTO
{
    public string BILL_NUM { get; set; }
    public string CUSTOMER_NAME { get; set; }
    public string LOCATION_CODE { get; set; }
    public string ADDRESS { get; set; }
    public string TOTAL_BILL_AMOUNT { get; set; }
    public string EnergyCost { get; set; }
    public string VatAmount { get; set; }
}

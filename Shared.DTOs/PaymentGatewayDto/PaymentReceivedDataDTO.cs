using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.PaymentGatewayDto
{
    public class PaymentReceivedDataDTO
    {
        public string? tran_id { get; set; }
        public string? cust_no { get; set; }
        public string? bill_no { get; set; }
        public string? error { get; set; }
        public string? bank_tran_id { get; set; }
        public string? amount { get; set; }
        public string? card_type { get; set; }
        public string? card_no { get; set; }
        public string? card_issuer { get; set; }
        public string? card_brand { get; set; }
        public string? card_sub_brand { get; set; }
        public string? card_issuer_country { get; set; }
        public string? verify_sign { get; set; }
        public string? verify_key { get; set; }
        public string? verify_sign_sha2 { get; set; }
        public string? val_id { get; set; }
        public string? gateway_status { get; set; }
        public string? store_amount { get; set; }
        public string? risk_level { get; set; }
        public string? risk_title { get; set; }
        public string? user { get; set; }
        public string? success { get; set; }
        public string? tran_date { get; set; }
    }
}

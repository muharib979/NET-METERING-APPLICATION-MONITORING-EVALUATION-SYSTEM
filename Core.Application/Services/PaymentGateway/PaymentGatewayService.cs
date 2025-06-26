using Core.Domain.PaymnetGateway;
using Microsoft.AspNetCore.Http;
using Nancy;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services.PaymentGateway
{
    public class PaymentGatewayService
    {
        public  NameValueCollection PostData(decimal? totalAmount, string baseUrl
            , string confirmUrl, string failedUrl, string cencelUrl, string customerNumber,string billNumber, RequestData postModel)
        {
            NameValueCollection data = new NameValueCollection();

            data.Add("total_amount", $"{totalAmount}");
            string tran_id = Guid.NewGuid().ToString();
            data.Add("tran_id", tran_id);
            data.Add("success_url", $"{baseUrl}{confirmUrl}");
            data.Add("fail_url", baseUrl + failedUrl);
            data.Add("cancel_url", baseUrl + cencelUrl);

            data.Add("version", "3.00");
            data.Add("cus_name", $"{postModel.CUST_NAME}");
            data.Add("cus_email", "infonet.associates.bd.dhk@gmail.com");
            data.Add("cus_add1", $"{postModel.LOCATION_CODE}");
            data.Add("cus_add2", $"{postModel.CUST_ADDRESS}");
            data.Add("cus_city", "");
            data.Add("cus_state", "S");
            data.Add("cus_postcode", "");
            data.Add("cus_country", "Bangladesh");
            data.Add("cus_phone", "01");
            data.Add("cus_fax", "");
            data.Add("ship_name", "");
            data.Add("ship_add1", "Address Line On");
            data.Add("ship_add2", "Address Line Tw");
            data.Add("ship_city", "City Nam");
            data.Add("ship_state", "State Nam");
            data.Add("ship_postcode", "Post Cod");
            data.Add("ship_country", "Countr");
            data.Add("value_a", $"{postModel.CUSTOMER_NUM}");
            data.Add("value_b", $"{postModel.BILL_NO}");
            data.Add("value_c", $"{postModel.CUST_NAME}");
            data.Add("value_d", "ref00");
            data.Add("emi_instalment", "");
            data.Add("emi_amount", "");
            data.Add("emi_description", "");
            data.Add("emi_issuer", "");
            data.Add("account_details", "");
            data.Add("account_details", "");
            data.Add("shipping_method", "NO");
            data.Add("num_of_item", "1");
            data.Add("product_name", $"{postModel.CUSTOMER_NUM}");
            data.Add("product_profile", $"{postModel.BILL_NO}");
            data.Add("product_category", "Demo");

            return data;
        }

        public NameValueCollection ConsumerPostData(decimal? totalAmount, string baseUrl
            , string confirmUrl, string failedUrl, string cencelUrl, string customerNumber, string billNumber, Core.Domain.MISCBILL.ConsumerBill postModel)
        {
            NameValueCollection data = new NameValueCollection();

            data.Add("total_amount", $"{totalAmount}");
            string tran_id = Guid.NewGuid().ToString();
            data.Add("tran_id", tran_id);
            data.Add("success_url", $"{baseUrl}{confirmUrl}");
            data.Add("fail_url", baseUrl + failedUrl);
            data.Add("cancel_url", baseUrl + cencelUrl);

            data.Add("version", "3.00");
            data.Add("cus_name", $"{postModel.CUST_NAME}");
            data.Add("cus_email", "infonet.associates.bd.dhk@gmail.com");
            data.Add("cus_add1", $"{postModel.LOCATION_CODE}");
            data.Add("cus_add2", $"{postModel.CUST_ADDRESS}");
            data.Add("cus_city", "");
            data.Add("cus_state", "S");
            data.Add("cus_postcode", "");
            data.Add("cus_country", "Bangladesh");
            data.Add("cus_phone", "01");
            data.Add("cus_fax", "");
            data.Add("ship_name", "");
            data.Add("ship_add1", "Address Line On");
            data.Add("ship_add2", "Address Line Tw");
            data.Add("ship_city", "City Nam");
            data.Add("ship_state", "State Nam");
            data.Add("ship_postcode", "Post Cod");
            data.Add("ship_country", "Countr");
            data.Add("value_a", $"{postModel.CUSTOMER_NUM}");
            data.Add("value_b", $"{postModel.BILL_NO}");
            data.Add("value_c", $"{postModel.CUST_NAME}");
            data.Add("value_d", "ref00");
            data.Add("emi_instalment", "");
            data.Add("emi_amount", "");
            data.Add("emi_description", "");
            data.Add("emi_issuer", "");
            data.Add("account_details", "");
            data.Add("account_details", "");
            data.Add("shipping_method", "NO");
            data.Add("num_of_item", "1");
            data.Add("product_name", $"{postModel.CUSTOMER_NUM}");
            data.Add("product_profile", $"{postModel.BILL_NO}");
            data.Add("product_category", "Demo");

            return data;
        }

        public async Task<PaymentReceivedDataDTO> PaymentReceivedData(HttpRequest Request, string status)
        {
            PaymentReceivedDataDTO model = new PaymentReceivedDataDTO();
            model.tran_id = Request.Form["tran_id"];
            model.cust_no = Request.Form["value_a"];
            model.bill_no = Request.Form["value_b"];
            model.error = Request.Form["error"];
            model.bank_tran_id = Request.Form["bank_tran_id"];
            model.tran_date = Request.Form["tran_date"];
            model.amount = Request.Form["amount"];
            model.card_type = Request.Form["card_type"];
            model.card_no = Request.Form["card_no"];
            model.card_issuer = Request.Form["card_issuer"];
            model.card_brand = Request.Form["card_brand"];
            model.card_sub_brand = Request.Form["card_sub_brand"];
            model.card_issuer_country = Request.Form["card_issuer_country"];
            model.verify_sign = Request.Form["verify_sign"];
            model.verify_key = Request.Form["verify_key"];
            model.verify_sign_sha2 = Request.Form["verify_sign_sha2"];
            model.val_id = Request.Form["val_id"];
            model.gateway_status = Request.Form["status"];
            model.store_amount = Request.Form["store_amount"];
            model.risk_level = Request.Form["risk_level"];
            model.risk_title = Request.Form["risk_title"];
            model.user = Request.Form["value_c"];
            model.success = status;

            return model;

        }
    }
}

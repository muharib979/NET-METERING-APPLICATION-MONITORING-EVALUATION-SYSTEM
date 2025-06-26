using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Nagad
{
    public class GlobalVariables
    {
        public static string MerchantId = "683002007104225";

      
        //Generate Random Number
        static Random r = new Random();
        public static int RandomNumber = r.Next(100000000, 999999999); //Randam Number should be less than 20 char

        //Initialize API URL
        public static string InitializeAPI = "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/initialize/";
        //Example:  http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/initialize/" + this.merchantId + "/" + orderId;
        public static string CheckOutAPI = "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/complete/";
        //Example "http://sandbox.mynagad.com:10080/remote-payment-gateway-1.0/api/dfs/check-out/complete/" + sensitiveDataInitializeResponse.getPaymentReferenceId();

    }
}

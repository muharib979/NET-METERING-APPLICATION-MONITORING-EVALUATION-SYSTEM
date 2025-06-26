using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.ClientApiSetup
{
    public class PostPaidToPrePaidEndPoint
    {
        public class EndPointSetting
        {
            public string ApiUrl = "http://accountapi.busbd.com.bd";
            public string Username = "zubayer";
            public string Password = "m7PM/lj+MYKoUcaydxQQe6Ez6Qal5N5DQAArpAmFcgOn+TLK3tN+VA==";
            //static long ticks = DateTime.UtcNow.Ticks;
            static int rand = new Random().Next(10000000, 100000000);
            //static long uniqueId = ticks;
           static Guid guid = Guid.NewGuid();

            // Convert the GUID to a byte array
           static byte[] bytes = guid.ToByteArray();

            // Convert the byte array to a long integer
            public static long integer = BitConverter.ToInt64(bytes, 0);
            // Ensure that the integer is positive
            public static long  integer1 = Math.Abs(integer);
            // Trim the integer to 14 digits
            public static long  integer2 = integer1 % 100000000000000;
            public string tran_id = integer2.ToString();
            //public static string Sku = "28eec8f69f40e4fca341aaef740589ad";
        }
    }
}

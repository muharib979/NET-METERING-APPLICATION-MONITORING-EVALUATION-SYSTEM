namespace AdminSystem.API.Uploades
{
    public class MessageGateways
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly string _senderId;

        public MessageGateways(string apiUrl, string apiKey, string senderId)
        {
            _httpClient = new HttpClient();
            _apiUrl = apiUrl;
            _apiKey = apiKey;
            _senderId = senderId;
        }

        public async Task SendSmsAsync(string mobile, string smsMessage)
        {
            var requestBody = new
            {
                to = mobile,
                text = smsMessage,
                sender = _senderId
            };

            //var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl);
            //request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            //request.Content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");

            //var response = await _httpClient.SendAsync(request);

            var apiUrl = $"{_apiUrl}?api_token={_apiKey}&senderid={_senderId}&message={smsMessage}&contact_number={mobile}";

            // Make a GET request to the API URL
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("SMS sent successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to send SMS. Status code: {response.StatusCode}");
            }

        }

        //public bool SendMessage(string mobile, string smsMessage)
        //{

        //    string status = "";
        //    try
        //    {

        //        // var result = sms.SendTextMultiMessage("BPDB", "Bpdb@1234", "8801847050272", mobile, smsMessage);

        //    }
        //    catch
        //    {
        //        status = "Sending Error";
        //    }
        //    return true;
        //}
    }
}

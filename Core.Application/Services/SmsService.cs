using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    public class SmsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public SmsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiUrl = "http://api.smsinbd.com/sms-api/sendsms";
        }

        public async Task<bool> SendSmsAsync(string apiKey, string senderId, string message, string contactNumber)
        {
            try
            {
                // Build the API URL with parameters
                var apiUrl = $"{_apiUrl}?api_token={apiKey}&senderid={senderId}&message={message}&contact_number={contactNumber}";

                // Make a GET request to the API URL
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // Check if the request was successful
                response.EnsureSuccessStatusCode();

                // SMS sent successfully
                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return false;
            }
        }
    }
}

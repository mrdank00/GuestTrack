using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace GuestTrack
{
   

    public class BulkSmsSender
    {
        private string smsGatewayUrl;
        private string apiKey;

        public BulkSmsSender(string smsGatewayUrl, string apiKey)
        {
            this.smsGatewayUrl = smsGatewayUrl;
            this.apiKey = apiKey;
        }

        public void SendBulkSms(string sender, string message, List<string> recipients)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    // Prepare the request payload
                    var payload = new
                    {
                        sender = sender,
                        message = message,
                        recipients = recipients
                    };

                    // Convert the payload to JSON
                    string payloadJson = JsonConvert.SerializeObject(payload);

                    // Set the request content type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["Api-Key"] = apiKey;

                    // Send the request and get the response
                    string response = client.UploadString(smsGatewayUrl, "POST", payloadJson);

                    // Handle the response
                    // You can parse and process the response here
                    Console.WriteLine($"SMS sent successfully. Response: {response}");
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine($"Failed to send SMS. Error: {ex.Message}");
                }
            }
        }
    }

}

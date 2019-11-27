using Newtonsoft.Json;
using Portal.Shared.Services.Notifications.Payloads;
using Portal.Shared.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Shared.Services.Notifications
{
    public interface IEmailNotification
    {
        Task<EmailResponse> SendAsync(string title, string rawMessage, string htmlMessage, string[] recipients);
    }

    public class EmailNotification : IEmailNotification
    {
        public string ApiKey { get; private set; }
        public string Sender { get; private set; }
        public string ReplyTo { get; private set; }
        public string Host { get; private set; }

        public EmailNotification(string apiKey, string sender, string replyTo, string host)
        {
            ApiKey = apiKey;
            Sender = sender;
            ReplyTo = replyTo;
            Host = host;
        }

        public async Task<EmailResponse> SendAsync(string title, string rawMessage, string htmlMessage, string[] recipients)
        {
            if (!string.IsNullOrWhiteSpace(title) &&
                (!string.IsNullOrWhiteSpace(rawMessage) || !string.IsNullOrWhiteSpace(htmlMessage)) &&
                recipients.AnyAndNotNull())
            {
                var to = new List<string>(recipients);

                var payload = new EmailPayload
                {
                    ApiKey = ApiKey,
                    To = to,
                    Sender = Sender,
                    Subject = title,
                    Text = rawMessage,
                    Html = htmlMessage,
                    Headers = new List<EmailPayloadCustomHeader>
                    {
                        new EmailPayloadCustomHeader
                        {
                            Header = "Reply-To",
                            Value = ReplyTo
                        }
                    }
                };

                using (var request = new HttpRequestMessage(HttpMethod.Post, "email/send"))
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient() { BaseAddress = new Uri(Host) })
                    {
                        var response = await httpClient.SendAsync(request);

                        var error = await response.Content.ReadAsStringAsync();

                        try
                        {
                            var status = response.EnsureSuccessStatusCode();
                        }
                        catch (Exception ex)
                        {
                            return new EmailResponse
                            {
                                Message = ex.Message
                            };
                        }
                    }
                }

                return new EmailResponse();
            }

            return new EmailResponse
            {
                Message = "Pusta wiadomość nie będzie wysłana"
            };
        }
    }

    public class EmailResponse
    {
        public string Message { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(Message);
    }
}
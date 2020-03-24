using System.Collections.Specialized;
using System.Net;

namespace Portal.Shared.Services.Notifications
{
    public interface IPushoverNotification
    {
        void Send(string title, string message);
    }

    public class PushoverNotification : IPushoverNotification
    {
        public string Token { get; private set; }
        public string Recipients { get; private set; }
        public string Endpoint { get; private set; }

        public PushoverNotification(
            string token,
            string recipients,
            string endpoint)
        {
            Token = token;
            Recipients = recipients;
            Endpoint = endpoint;
        }

        public void Send(string title, string message)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(message))
            {
                var parameters = new NameValueCollection
                {
                    { "token", Token },
                    { "user", Recipients },
                    { "message", message },
                    { "title", title },
                    { "html", "1" }
                };

                using (var client = new WebClient())
                {
                    client.UploadValues(Endpoint, parameters);
                }
            }
        }
    }
}
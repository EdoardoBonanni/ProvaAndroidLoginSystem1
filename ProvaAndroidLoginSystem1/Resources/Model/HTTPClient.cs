using System.Net.Http;

namespace ProvaAndroidLoginSystem1.Resources.Model
{
    public class HTTPClient
    {
        HttpClient client;

        public HTTPClient()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public HttpClient Client { get { return this.client; } }
    }
}
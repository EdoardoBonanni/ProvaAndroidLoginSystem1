using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
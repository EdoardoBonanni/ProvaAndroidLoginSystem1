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

namespace p2p_project.Resources
{
    interface ISocket
    {
        int Connect();

        void Send(string packet);

        void Receive();

        void receiveCallback(IAsyncResult res);

        void End();
    }
}
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
using Java.Lang;
using Java.Net;
using System.Net.Sockets;

namespace p2p_project.Resources
{
    class ClientSocket
    {
        private int port = 9876;
        private InetAddress ip;
        private System.Net.Sockets.Socket socket;

        public ClientSocket(InetAddress ip)
        {
            this.ip = ip;
        }

        public int Connect()
        {
            socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IAsyncResult result = socket.BeginConnect(ip.HostAddress, port, null, null);

            bool success = result.AsyncWaitHandle.WaitOne(30000, true);
            if (!success)
            {
                return -1;
            }
            return 1;
        }
        public void Send()
        {

        }

        public void Receive()
        {

        }

        public void End()
        {

        }
    }
}
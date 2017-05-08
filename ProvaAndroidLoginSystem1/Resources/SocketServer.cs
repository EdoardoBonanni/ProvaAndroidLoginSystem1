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
using System.Net;
using System.Threading.Tasks;

namespace p2p_project.Resources
{
    class SocketServer
    {
        private readonly int port = 9876;
        private TcpListener serverSocket;
        private Java.Net.Socket client;

        public int Connect()
        {
            serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            var result = serverSocket.BeginAcceptTcpClient(new AsyncCallback(CreateThread), serverSocket);
            bool success = result.AsyncWaitHandle.WaitOne(20000, true);
            if (!success)
            {
                serverSocket.Stop();
                return -1;
            }
            return 1;
        }

        private void CreateThread(IAsyncResult ar)
        {

            var client = serverSocket.EndAcceptTcpClient(ar);
        }

        public void Send()
        {

        }

        public void Receive()
        {

        }

        public void End()
        {
            serverSocket.Stop();
        }
    }
}
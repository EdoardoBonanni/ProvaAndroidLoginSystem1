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
using System.Threading.Tasks;

namespace p2p_project.Resources
{
    class SocketServer
    {
        private readonly int port = 9876;
        private ServerSocket serverSocket;
        private Socket client;

        public int Connect()
        {
            try
            {
                serverSocket = new ServerSocket(port);
                client = serverSocket.Accept();
                return 1;
            }catch(System.Exception ex)
            {
                return -1;
            }
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
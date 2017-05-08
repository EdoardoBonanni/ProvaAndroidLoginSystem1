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
using System.Net;
using System.Net.Sockets;

namespace p2p_project.Resources
{
    class ClientSocket
    {
        private int port = 9876;
        private InetAddress ip;
        private TcpClient client;

        public ClientSocket(InetAddress ip)
        {
            this.ip = ip;
        }

        public int Connect()
        {
            client = new TcpClient();
            try
            {
                if (!client.ConnectAsync(IPAddress.Parse(ip.HostAddress), port).Wait(10000))
                {
                    client.Close();
                    return -1;
                }
            }
            catch(System.Exception ex)
            {
                client.Close();
                return -1;
            }
            
            var networkStream = client.GetStream();
            //creare thread con networkStream
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
            client.Close();
        }
    }
}
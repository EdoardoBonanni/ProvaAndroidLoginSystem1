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
using ProvaAndroidLoginSystem1.Resources;

namespace p2p_project.Resources
{
    class ClientSocket : ISocket
    {
        private int port = 9876;
        private InetAddress ip;
        private TcpClient client;
        private NetworkStream networkStream;
        private Thread receive;
        private PacketManager packetManager;

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
                    return -1;
                }
            }
            catch(System.Exception ex)
            {
                return -1;
            }
            
            networkStream = client.GetStream();
            packetManager = new PacketManager(this);
            receive = new Thread(Receive);
            receive.Start();
            return 1;
        }

        public async void Send(string packet)
        {
            byte[] data = Encoding.UTF8.GetBytes(packet);
            await networkStream.WriteAsync(data, 0, data.Length);
        }

        public void Receive()
        {
            byte[] data = new byte[4096];
            /*int responseCount = await networkStream.ReadAsync(data, 0, data.Length);
            string responseData = System.Text.Encoding.ASCII.GetString(data, 0, responseCount);
            Toast.MakeText(Application.Context, responseData, ToastLength.Long);*/
            networkStream.BeginRead(data, 0, data.Length, new AsyncCallback(receiveCallback), data);
        }

        public void receiveCallback(IAsyncResult res)
        {
            byte[] data = (byte[]) res.AsyncState;
            int responseCount = networkStream.EndRead(res);
            if(responseCount > 0)
            {
                string buff = System.Text.Encoding.UTF8.GetString(data, 0, responseCount);
                packetManager.Unpack(buff);
                networkStream.BeginRead(data, 0, data.Length, new AsyncCallback(receiveCallback), data);
            }
        }

        public void End()
        {
            if(receive != null)
                receive.Dispose();
            client.Close();
        }
    }
}
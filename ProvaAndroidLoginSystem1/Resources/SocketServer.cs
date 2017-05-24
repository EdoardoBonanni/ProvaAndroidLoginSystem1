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
        private TcpClient client;
        private NetworkStream networkStream;
        private Thread receive;

        public int Connect()
        {
            serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            var result = serverSocket.BeginAcceptTcpClient(null, null);
            bool success = result.AsyncWaitHandle.WaitOne(20000, true);
            if (!success)
            {
                return -1;
            }
            client = serverSocket.EndAcceptTcpClient(result);
            networkStream = client.GetStream();
            receive = new Thread(Receive);
            receive.Start();
            return 1;
        }

        public async void Send(string packet)
        {
            byte[] data = Encoding.ASCII.GetBytes(packet);
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
            byte[] data = (byte[])res.AsyncState;
            int responseCount = networkStream.EndRead(res);
            if (responseCount > 0)
            {
                string buff = System.Text.Encoding.ASCII.GetString(data, 0, responseCount);
                networkStream.BeginRead(data, 0, data.Length, new AsyncCallback(receiveCallback), data);
            }
        }

        public void End()
        {
            if (receive != null)
                receive.Dispose();
            serverSocket.Stop();
        }
    }
}
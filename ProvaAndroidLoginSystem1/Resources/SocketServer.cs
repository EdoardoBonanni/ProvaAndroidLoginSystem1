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
            TcpListener serverSocket = new TcpListener(IPAddress.Any, port);
            serverSocket.Start();
            var result = serverSocket.BeginAcceptTcpClient(null, null);
            bool success = result.AsyncWaitHandle.WaitOne(20000, true);
            if (!success)
            {
                serverSocket.Stop();
                return -1;
            }
            client = serverSocket.EndAcceptTcpClient(result);
            networkStream = client.GetStream();
            receive = new Thread(Receive);
            receive.Start();
            return 1;
        }

        public async void Send()
        {
            byte[] data = Encoding.ASCII.GetBytes("Primo test");
            await networkStream.WriteAsync(data, 0, data.Length);
        }

        public async void Receive()
        {
            while (true)
            {
                byte[] data = new byte[2048];
                int responseCount = await networkStream.ReadAsync(data, 0, data.Length);
                string responseData = System.Text.Encoding.ASCII.GetString(data, 0, responseCount);
                Toast.MakeText(Application.Context, responseData, ToastLength.Long);
            }
        }

        public void End()
        {
            receive.Dispose();
            serverSocket.Stop();
        }
    }
}
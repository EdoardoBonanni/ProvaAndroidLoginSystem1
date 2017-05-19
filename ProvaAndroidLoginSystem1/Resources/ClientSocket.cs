﻿using System;
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
        private NetworkStream networkStream;
        private Thread receive;

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
            client.Close();
        }
    }
}
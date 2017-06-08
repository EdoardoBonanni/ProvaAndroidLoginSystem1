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
using System.Text.RegularExpressions;

namespace p2p_project.Resources
{
    class SocketServer : ISocket
    {
        private readonly int port = 9876;
        private TcpListener serverSocket;
        private TcpClient client;
        private NetworkStream networkStream;
        private Thread receive;
        private PacketManager packetManager;
        private string appendBuff;
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
            byte[] data = new byte[10000];
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
                string buff = Encoding.UTF8.GetString(data, 0, responseCount);
                var token = split(buff, '}');

                foreach (var tok in token)
                {
                    appendBuff += tok;
                    if (appendBuff.Contains("{") && appendBuff.Contains("}"))
                    {
                        packetManager.Unpack(appendBuff);
                        appendBuff = "";
                    }
                }
                networkStream.BeginRead(data, 0, data.Length, new AsyncCallback(receiveCallback), data);
            }
        }

        private string[] split(string buffer, char separator)
        {
            var chars = buffer.ToCharArray();
            List<string> token = new List<string>();
            string tok = "";

            foreach (var ch in chars)
            {
                tok += ch;
                if (ch.Equals(separator))
                {
                    token.Add(tok);
                    tok = "";
                }
            }

            if (!tok.Equals(""))
            {
                token.Add(tok);
            }

            var tokens = token.ToArray();
            return tokens;
        }

        public void End()
        {
            if (receive != null)
                receive.Dispose();
            serverSocket.Stop();
        }
    }
}
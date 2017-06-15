using System;
using System.Collections.Generic;
using System.Text;
using Java.Lang;
using Java.Net;
using System.Net;
using System.Net.Sockets;

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
        private string appendBuff;

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
            if(receive != null)
                receive.Dispose();
            client.Close();
        }
    }
}
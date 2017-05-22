using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Net.Wifi.P2p.WifiP2pManager;
using Java.Net;
using ProvaAndroidLoginSystem1;
using ProvaAndroidLoginSystem1.Resources;

namespace p2p_project.Resources
{
    class ConnectionInfoListener : Java.Lang.Object, IConnectionInfoListener
    {
        private MainActivity main;
        private static SocketServer server;
        private static ClientSocket client;
        public ConnectionInfoListener(MainActivity main)
        {
            this.main = main;
        }

        public void OnConnectionInfoAvailable(WifiP2pInfo info)
        {
            // InetAddress from WifiP2pInfo struct.
            InetAddress groupOwnerAddress = info.GroupOwnerAddress;

            if (info.IsGroupOwner)
            {
                server = new SocketServer();
                int serverConnected = server.Connect();
                if (serverConnected == 1)
                {
                    isServer = true;
                    main.changeActivity();
                }
                else
                {
                    server.End();
                    main.DisconnectP2p();
                    Toast.MakeText(Application.Context, "L'altro dispositivo non ha l'app aperta", ToastLength.Long);
                }
            }
            else
            {
                client = new ClientSocket(groupOwnerAddress);
                int clientConnected = client.Connect();
                if(clientConnected == 1)
                {
                    isServer = false;
                    main.changeActivity();
                }
                else
                {
                    client.End();
                    main.DisconnectP2p();
                    Toast.MakeText(Application.Context, "L'altro dispositivo non ha l'app aperta", ToastLength.Long);
                }
            }
        }

        public static bool isServer { get; set; }
        public static SocketServer Server { get { return server; } }

        public static ClientSocket Client { get { return client; } }

    }
}
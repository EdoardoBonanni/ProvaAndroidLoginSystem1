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

namespace p2p_project.Resources
{
    class ConnectionInfoListener : Java.Lang.Object, IConnectionInfoListener
    {
        private MainActivity main;
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
                SocketServer server = new SocketServer();
                int serverConnected = server.Connect();
                if (serverConnected == 1)
                {
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
                ClientSocket client = new ClientSocket(groupOwnerAddress);
                int clientConnected = client.Connect();
                if(clientConnected == 1)
                {
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
    }
}
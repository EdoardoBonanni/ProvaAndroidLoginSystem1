
using Android.App;
using Android.Net.Wifi.P2p;
using Android.Widget;
using static Android.Net.Wifi.P2p.WifiP2pManager;
using Java.Net;
using ProvaAndroidLoginSystem1;

namespace p2p_project.Resources
{
    class ConnectionInfoListener : Java.Lang.Object, IConnectionInfoListener
    {
        private MainActivity main;
        private static ISocket socket;
        public ConnectionInfoListener(MainActivity main)
        {
            this.main = main;
        }

        public void OnConnectionInfoAvailable(WifiP2pInfo info)
        {
            // InetAddress from WifiP2pInfo struct.
            InetAddress groupOwnerAddress = info.GroupOwnerAddress;

            PacketManager.usernameReceived += delegate
            {
                main.changeActivity();
            };

            if (info.IsGroupOwner)
            {
                socket = new SocketServer();
            }
            else
            {
                socket = new ClientSocket(groupOwnerAddress);
            }

            int socketConnected = socket.Connect();
            if (socketConnected == 1)
            {
                socket.Send(PacketManager.PackUsername(MainActivity.retrieveLocal("Username") ?? ""));
            }
            else
            {
                socket.End();
                main.DisconnectP2p();
                Toast.MakeText(Application.Context, "L'altro dispositivo non ha l'app aperta", ToastLength.Long);
            }
        }
        
        public static ISocket Socket { get { return socket; } }

    }
}
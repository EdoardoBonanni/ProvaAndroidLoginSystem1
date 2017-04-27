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
using Android.Net.Wifi.P2p;
using static Android.Net.Wifi.P2p.WifiP2pManager;
using ProvaAndroidLoginSystem1;

namespace p2p_project.Resources
{
    class PeerListener : Java.Lang.Object, IPeerListListener
    {
        private List<WifiP2pDevice> peers = new List<WifiP2pDevice>();
        private MainActivity mainActivity;

        public PeerListener(MainActivity activity)
        {
            this.mainActivity = activity;
        }

        public void OnPeersAvailable(WifiP2pDeviceList peerList)
        {
            List<WifiP2pDevice> refreshedPeers = peerList.DeviceList.ToList();
            if (!refreshedPeers.Equals(peers))
            {
                peers.Clear();
                peers.AddRange(refreshedPeers);

                // If an AdapterView is backed by this data, notify it
                // of the change.  For instance, if you have a ListView of
                // available peers, trigger an update. 

                // Notify the adapter.
                mainActivity.test();

                // Perform any other updates needed based on the new list of
                // peers connected to the Wi-Fi P2P network.
            }
            if (peers.Count == 0)
            {
                Toast.MakeText(Application.Context, "Nessun peer trovato", ToastLength.Long).Show();
                return;
            }
        }

        public List<WifiP2pDevice> Peers { get { return this.peers; } }
    }
}
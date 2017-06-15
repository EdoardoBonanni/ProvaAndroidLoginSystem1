using System.Collections.Generic;
using System.Linq;
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

                // Notify the adapter.
                mainActivity.notifyAdapter();
            }
        }

        public List<WifiP2pDevice> Peers { get { return this.peers; } set { this.peers = value; } }
    }
}
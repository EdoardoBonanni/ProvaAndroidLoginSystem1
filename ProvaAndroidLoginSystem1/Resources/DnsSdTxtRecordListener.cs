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

namespace p2p_project.Resources
{
    class DnsSdTxtRecordListener : Java.Lang.Object, IDnsSdTxtRecordListener
    {
        private static Dictionary<string, string> peers = new Dictionary<string, string>();
        public void OnDnsSdTxtRecordAvailable(string fullDomainName, IDictionary<string, string> txtRecordMap, WifiP2pDevice srcDevice)
        {
            peers.Add(srcDevice.DeviceAddress, txtRecordMap["Client Name"]);
        }

        public static Dictionary<string, string> Peers { get; set; }
    }
}
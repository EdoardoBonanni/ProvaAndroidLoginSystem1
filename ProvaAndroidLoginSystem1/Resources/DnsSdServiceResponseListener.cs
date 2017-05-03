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
    class DnsSdServiceResponseListener : Java.Lang.Object, IDnsSdServiceResponseListener
    {
        private List<WifiP2pDevice> serviceDevices = new List<WifiP2pDevice>();
        public void OnDnsSdServiceAvailable(string instanceName, string registrationType, WifiP2pDevice srcDevice)
        {
            if(instanceName.Equals("Chat P2p"))
            {
                srcDevice.DeviceName = DnsSdTxtRecordListener.Peers.ContainsKey(srcDevice.DeviceAddress) ? DnsSdTxtRecordListener.Peers[srcDevice.DeviceAddress] : srcDevice.DeviceName;
                serviceDevices.Add(srcDevice);
            }
        }

        public List<WifiP2pDevice> ServiceDevices { get { return this.serviceDevices; } set { this.serviceDevices = value; } }
    }
}
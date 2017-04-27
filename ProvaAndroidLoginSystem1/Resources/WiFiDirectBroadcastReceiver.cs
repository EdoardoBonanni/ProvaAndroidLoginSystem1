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
using Android.Net.Wifi.P2p;
using ProvaAndroidLoginSystem1.Resources;
using ProvaAndroidLoginSystem1;
using static Android.Net.Wifi.P2p.WifiP2pManager;

namespace p2p_project.Resources
{
    /**
 * A BroadcastReceiver that notifies of important Wi-Fi p2p events.
 */
    class WiFiDirectBroadcastReceiver : BroadcastReceiver{

        private WifiP2pManager mManager;
        private WifiP2pManager.Channel mChannel;
        private MainActivity mActivity;
        private PeerListener peerListener;

        public WiFiDirectBroadcastReceiver(WifiP2pManager manager, WifiP2pManager.Channel channel,
                MainActivity activity, PeerListener peerListener)
        {
            this.mManager = manager;
            this.mChannel = channel;
            this.mActivity = activity;
            this.peerListener = peerListener;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            String action = intent.Action;

            if (WifiP2pManager.WifiP2pStateChangedAction.Equals(action))
            {
                // Check to see if Wi-Fi is enabled and notify appropriate activity

                var state = intent.GetIntExtra(WifiP2pManager.ExtraWifiState, -1);
                if (state == (int)WifiP2pState.Enabled)
                    // Wifi Direct mode is enabled
                    mActivity.IsWifiP2PEnabled = true;
                else
                {
                    mActivity.IsWifiP2PEnabled = false;
                    //mActivity.ResetData(); Cancella tutti i peer scoperti
                }
            }
            else if (WifiP2pManager.WifiP2pPeersChangedAction.Equals(action))
            {
                // Call WifiP2pManager.requestPeers() to get a list of current peers

                // request available peers from the wifi p2p manager. This is an
                // asynchronous call and the calling activity is notified with a
                // callback on PeerListListener.onPeersAvailable()
                if (mManager != null)
                {
                    mManager.RequestPeers(mChannel, peerListener);
                }
            }
            /*
            else if (WifiP2pManager.WifiP2pDiscoveryChangedAction.Equals(action))
            {

                var state = intent.GetIntExtra(WifiP2pManager.ExtraDiscoveryState, -1);

                if(state == WifiP2pManager.WifiP2pDiscoveryStarted)
                {
                    // Call WifiP2pManager.requestPeers() to get a list of current peers

                    // request available peers from the wifi p2p manager. This is an
                    // asynchronous call and the calling activity is notified with a
                    // callback on PeerListListener.onPeersAvailable()
                    if (mManager != null)
                    {
                        mManager.RequestPeers(mChannel, peerListener);
                    }
                }
            }
            */
            else if (WifiP2pManager.WifiP2pConnectionChangedAction.Equals(action))
            {
                // Respond to new connection or disconnections

                if (mManager == null)
                    return;

                var networkInfo = (Android.Net.NetworkInfo)intent.GetParcelableExtra(WifiP2pManager.ExtraNetworkInfo);

                if (networkInfo.IsConnected)
                {
                    // we are connected with the other device, request connection
                    // info to find group owner IP
                    mManager.RequestConnectionInfo(mChannel, null);
                }
                else
                {
                    // It's a disconnect
                    //mActivity.ResetData();
                }
            }
            else if (WifiP2pManager.WifiP2pThisDeviceChangedAction.Equals(action))
            {
                // Respond to this device's wifi state changing
                
            }
        }
    }
}
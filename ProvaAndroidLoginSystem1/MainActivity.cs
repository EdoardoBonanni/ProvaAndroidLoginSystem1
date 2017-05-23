using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Java.Lang;
using ProvaAndroidLoginSystem1.Resources;
using Android.Content;
using ProvaAndroidLoginSystem1.Resources.Model;
using p2p_project;
using Android.Net.Wifi.P2p;
using p2p_project.Resources;
using Android.Net.Wifi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProvaAndroidLoginSystem1
{
    [Activity(Label = "Chat P2P", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static HTTPClient client;
        private Button mBtnSearch;
        private Button mBtnCancel;
        private ListView mLstPeers;

        public bool IsWifiP2PEnabled { get; set; }
        public bool IsConnected { get; set; }

        private WifiP2pManager manager;
        private IntentFilter intentFilter;
        private WifiP2pManager.Channel channel;
        private BroadcastReceiver receiver;
        private PeerListener peerListener;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            client = new HTTPClient();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mBtnSearch = FindViewById<Button>(Resource.Id.btnP2pSearch);
            mBtnCancel = FindViewById<Button>(Resource.Id.btnCancelSearch);
            mBtnCancel.Visibility = Android.Views.ViewStates.Invisible;
            mLstPeers = FindViewById<ListView>(Resource.Id.lstPeers);

            mBtnSearch.Click += mBtnSearch_Click;
            mBtnCancel.Click += mBtnCancel_Click;
            mLstPeers.ItemClick += mLstPeers_ItemClick;

            intentFilter = new IntentFilter();
            intentFilter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            intentFilter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            intentFilter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            intentFilter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);

            manager = (WifiP2pManager)GetSystemService(WifiP2pService);
            channel = manager.Initialize(this, MainLooper, null);
        }

        private void mLstPeers_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            WifiP2pDevice peerClicked = peerListener.Peers[e.Position];

            WifiP2pConfig config = new WifiP2pConfig();

            config.DeviceAddress = peerClicked.DeviceAddress;
            config.Wps.Setup = WpsInfo.Pbc;

            manager.Connect(channel, config, new ActionListener("Connessione..."));
        }

        private void mBtnCancel_Click(object sender, EventArgs e)
        {
            mBtnSearch.Visibility = Android.Views.ViewStates.Visible;
            mBtnCancel.Visibility = Android.Views.ViewStates.Invisible;
            manager.StopPeerDiscovery(channel, new ActionListener("Ricerca fermata"));
        }

        void mBtnSearch_Click(object sender, EventArgs e)
        {
            if (IsWifiP2PEnabled)
            {
                mBtnSearch.Visibility = Android.Views.ViewStates.Invisible;
                mBtnCancel.Visibility = Android.Views.ViewStates.Visible;
                manager.DiscoverPeers(channel, new ActionListener("Ricerca..."));
            }
            else
            {
                Toast.MakeText(this, "WiFi Direct non abilitato", ToastLength.Long).Show();
            }
        }

        public void notifyAdapter()
        {
            mLstPeers.Adapter = new PeersAdapter(this, peerListener.Peers);
        }

        public void changeActivity()
        {
            IsConnected = true;
            Intent Home = new Intent(this, typeof(HomeActivity));
            this.StartActivity(Home);
        }

        public void resetData()
        {
            peerListener.Peers = new List<WifiP2pDevice>();
            this.notifyAdapter();
        }

        protected override void OnResume()
        {
            base.OnResume();
            peerListener = new PeerListener(this);

            receiver = new WiFiDirectBroadcastReceiver(manager, channel, this, peerListener);
            RegisterReceiver(receiver, intentFilter);

            if(IsConnected)
            {
                DisconnectP2p();
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(receiver);
        }

        public void DisconnectP2p()
        {
            manager.RemoveGroup(channel, new ActionListener("Chiusura della connessione..."));
            IsConnected = false;
            manager.StopPeerDiscovery(channel, new ActionListener(""));
        }
    }
 }


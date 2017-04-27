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

namespace ProvaAndroidLoginSystem1
{
    [Activity(Label = "Chat P2P", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static HTTPClient client;
        private Button mBtnSignUp;
        private Button mBtnSignIn;

        public bool IsWifiP2PEnabled { get; set; }

        private WifiP2pManager manager;
        private IntentFilter intentFilter = new IntentFilter();
        private WifiP2pManager.Channel channel;
        private BroadcastReceiver receiver;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            client = new HTTPClient();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            /*
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);

            mBtnSignUp.Click += mBtnSignUp_Click;
            mBtnSignIn.Click += mbtnSignIn_Click;*/

            intentFilter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            intentFilter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            intentFilter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            intentFilter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);

            manager = (WifiP2pManager)GetSystemService(WifiP2pService);
            channel = manager.Initialize(this, MainLooper, null);
        }


        
        /*
        void mBtnSignUp_Click(object sender, EventArgs e)
        {
            Intent SignUp = new Intent(this, typeof(SignUpActivity));
            this.StartActivity(SignUp);
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

        }

        void mbtnSignIn_Click(object sender, EventArgs e)
        {
            Intent SignIn = new Intent(this, typeof(SignInActivity));
            this.StartActivity(SignIn);
        }
        */
    }
 }


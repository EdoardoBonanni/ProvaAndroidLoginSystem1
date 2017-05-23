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
using p2p_project;
using Android.Net.Wifi.P2p;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "Chat P2P")]
    class HomeActivity : Activity

    {

        private Button mBtnMessage;
        private Button mBtnFile;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.HomeLayout);

            mBtnMessage = FindViewById<Button>(Resource.Id.btnSendMessageHome);
            mBtnFile = FindViewById<Button>(Resource.Id.btnSendFileHome);

            mBtnMessage.Click += mbtnMessage_Click;
            mBtnFile.Click += mbtnFile_Click;
        }

        protected override void OnResume()
        {
            //Get the Id from Db
            base.OnResume();
        }

        void mbtnMessage_Click(object sender, EventArgs e)
        {
            Intent Message = new Intent(this, typeof(ChatActivity));
            this.StartActivity(Message);
        }

        void mbtnFile_Click(object sender, EventArgs e)
        {
            
        }
    }
}
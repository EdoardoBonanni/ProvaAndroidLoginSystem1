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
using p2p_project;
using p2p_project.Resources;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "Chat P2P")]
    public class ChatActivity : Activity
    {
        private Button btnSend;
        private SocketServer server;
        private ClientSocket client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ChatLayout);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            btnSend.Click += btnSend_Click;
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            if (ConnectionInfoListener.isServer)
            {
                SocketServer server = ConnectionInfoListener.Server;
                server.Send();
            }
            else
            {
                ClientSocket client = ConnectionInfoListener.Client;
                client.Send();
            }
        }
    }
}
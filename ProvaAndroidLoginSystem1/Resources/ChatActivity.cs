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
using p2p_project.Resources;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "Chat P2P")]
    public class ChatActivity : Activity
    {
        private ListView lstMessage;
        private Button btnSend;
        private EditText txtChat;
        private SocketServer server;
        private ClientSocket client;
        private ChatAdapter chatAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ChatLayout);
            lstMessage = FindViewById<ListView>(Resource.Id.lstMessages);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            txtChat = FindViewById<EditText>(Resource.Id.txtChat);
            btnSend.Click += btnSend_Click;
            chatAdapter = new ChatAdapter(this, new List<string>());
            lstMessage.Adapter = chatAdapter;
        }

        public void updateChat(string text)
        {
            chatAdapter.update(text);
            //inserimento nel Db
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            if (ConnectionInfoListener.isServer)
            {
                SocketServer server = ConnectionInfoListener.Server;
                server.Send(txtChat.Text);
            }
            else
            {
                ClientSocket client = ConnectionInfoListener.Client;
                client.Send(txtChat.Text);
            }
        }
    }
}
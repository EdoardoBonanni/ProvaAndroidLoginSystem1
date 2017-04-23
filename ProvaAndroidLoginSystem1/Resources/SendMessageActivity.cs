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

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "SendMessageActivity")]
    class SendMessageActivity : Activity
    {
        private ListView mlistViewChatMessage;
        private Button mbtnSendMessage;
        private EditText mtextMessageChat;
        private List<String> mlistMessageChat;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ChatLayout);
            mbtnSendMessage = FindViewById<Button>(Resource.Id.btnSendMessage);
            mlistViewChatMessage = FindViewById<ListView>(Resource.Id.listViewChatMessage);
            mtextMessageChat = FindViewById<EditText>(Resource.Id.txtChatMessage);
            mlistMessageChat = new List<String>();
            var adapter = new ListViewChatAdapter(this, mlistMessageChat);
            mlistViewChatMessage.Adapter = adapter;
            mbtnSendMessage.Click += mbtnSendMessage_Click;
            
        }

        void mbtnSendMessage_Click(object sender, EventArgs e)
        {
            if (!mtextMessageChat.Equals(""))
            {
                mlistMessageChat.Add(mtextMessageChat.Text);
                mtextMessageChat.Text = "";
            }
            var adapter = new ListViewChatAdapter(this, mlistMessageChat);
            mlistViewChatMessage.Adapter = adapter;
        }
    }
}
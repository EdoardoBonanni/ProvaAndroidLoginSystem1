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

namespace p2p_project
{
    class ChatAdapter : BaseAdapter
    {

        List<string> chat = new List<string>();
        Activity context;

        public ChatAdapter(Activity context, List<string> chat)
        {
            this.chat = chat;
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return this.chat[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Data_template_Peers_layout, parent, false);
            var Peer = view.FindViewById<TextView>(Resource.Id.txtPeer);

            if (chat[position] != null)
            {
                Peer.Text = chat[position];
            }

            return view;
        }

        public void update(string message)
        {
            this.chat.Add(message);
            NotifyDataSetChanged();
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return this.chat.Count;
            }
        }

    }
}
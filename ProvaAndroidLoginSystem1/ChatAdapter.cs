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

        List<Tuple<string, bool>> chat = new List<Tuple<string, bool>>();
        Activity context;

        public ChatAdapter(Activity context, List<Tuple<string, bool>> chat)
        {
            this.chat = chat;
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Data_template_layout, parent, false);
            var Peer = view.FindViewById<TextView>(Resource.Id.txtAdapter);

            if (chat[position] != null)
            {
                Peer.Text = chat[position].Item1;
                if(chat[position].Item2)
                {
                    Peer.Gravity = GravityFlags.Right;
                }
                else
                {
                    Peer.Gravity = GravityFlags.Left;
                }
            }

            return view;
        }

        public void update(string message, bool mine)
        {
            this.chat.Add(new Tuple<string, bool>(message, mine));
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
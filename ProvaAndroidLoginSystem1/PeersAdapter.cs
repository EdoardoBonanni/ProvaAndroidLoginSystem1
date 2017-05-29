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
using Android.Net.Wifi.P2p;

namespace p2p_project
{
    class PeersAdapter : BaseAdapter<WifiP2pDevice>
    {
        private List<WifiP2pDevice> peers = new List<WifiP2pDevice>();
        private Activity dialog;

        public PeersAdapter(Activity dialog, List<WifiP2pDevice> objects)
        {
            this.peers = objects;
            this.dialog = dialog;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public void update(List<WifiP2pDevice> list)
        {
            this.peers.Clear();
            this.peers.AddRange(list);
            NotifyDataSetChanged();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? dialog.LayoutInflater.Inflate(Resource.Layout.Data_template_layout, parent, false);
            var Peer = view.FindViewById<TextView>(Resource.Id.txtAdapter);

            if (peers[position] != null)
            {
                Peer.Text = peers[position].DeviceName;
            }

            return view;
        }

        public override int Count
        {
            get
            {
                return peers.Count;
            }
        }

        public override WifiP2pDevice this[int position]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
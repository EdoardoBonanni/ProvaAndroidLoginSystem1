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

namespace ProvaAndroidLoginSystem1
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtMessage;
    }
    public class ListViewChatAdapter : BaseAdapter
    {
        private Activity dialog;
        private List<String> lstChat;

        public ListViewChatAdapter(Activity dialog, List<String> lstChat)
        {
            this.dialog = dialog;
            this.lstChat = lstChat;
        }

        public override int Count
        {
            get
            {
                return lstChat.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstChat[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? dialog.LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, parent, false);
            var txtMessage = view.FindViewById<TextView>(Resource.Id.textView1);

            txtMessage.Text = lstChat[position];

            return view;
        }
    }
}
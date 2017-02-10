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
using Java.Lang;
using ProvaAndroidLoginSystem1.Resources.Model;

namespace ProvaAndroidLoginSystem1.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtname;
        public TextView txtemail;
        public TextView txtpassword;
    }
    public class ListViewAdapter:BaseAdapter
    {
        private DialogFragment dialog;
        private List<Person> lstPerson;
        
        public ListViewAdapter(DialogFragment dialog, List<Person> lstperson)
        {
            this.dialog = dialog;
            this.lstPerson = lstperson;
        }

        public override int Count
        {
            get
            {
                return lstPerson.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstPerson[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? dialog.Activity.LayoutInflater.Inflate(Resource.Layout.dialog_sign_up, parent, false);
            var txtname = view.FindViewById<TextView>(Resource.Id.txtFirstName);
            var txtemail = view.FindViewById<TextView>(Resource.Id.txtEmail);
            var txtpassword = view.FindViewById<TextView>(Resource.Id.txtPassword);

            txtname.Text = lstPerson[position].Firstname;
            txtemail.Text = lstPerson[position].Email;
            txtpassword.Text = lstPerson[position].Password;

            return view;
        }
    }
}
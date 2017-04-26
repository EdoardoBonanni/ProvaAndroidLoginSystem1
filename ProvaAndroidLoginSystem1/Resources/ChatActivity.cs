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
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity
    {
        private Button btnSend;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            // Create your application here
        }
    }
}
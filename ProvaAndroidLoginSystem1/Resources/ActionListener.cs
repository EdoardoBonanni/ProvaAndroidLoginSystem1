using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Net.Wifi.P2p.WifiP2pManager;
using ProvaAndroidLoginSystem1.Resources;
using ProvaAndroidLoginSystem1;

namespace p2p_project
{
    class ActionListener : Java.Lang.Object, IActionListener
    {
        private String message;

        public ActionListener(String msg)
        {
            this.message = msg;
        }

        public void OnFailure([GeneratedEnum] WifiP2pFailureReason reason)
        {
            Toast.MakeText(Application.Context, "Errore: " + reason.ToString(), ToastLength.Long).Show();
        }

        public void OnSuccess()
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}
using System;

using Android.App;
using Android.OS;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "SendMessageActivity")]
    class SendMessageActivity : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ChatLayout);
        }
    }
}
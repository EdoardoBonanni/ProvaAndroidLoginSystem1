using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Java.Lang;

namespace ProvaAndroidLoginSystem1
{
    [Activity(Label = "ProvaAndroidLoginSystem1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button mBtnSignUp;
        private ProgressBar mProgressBar;        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignUp.Click += (object sender, EventArgs args) => 
            {
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                Dialog_SignUp signUp = new Dialog_SignUp();
                signUp.Show(transaction, "Dialog fragment");
            };
            
        }
    }
}


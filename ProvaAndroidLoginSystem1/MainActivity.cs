using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Java.Lang;
using ProvaAndroidLoginSystem1.Resources;
using Android.Content;

namespace ProvaAndroidLoginSystem1
{
    [Activity(Label = "ProvaAndroidLoginSystem1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button mBtnSignUp;
        private ProgressBar mProgressBar;
        private Button mBtnSignIn;      
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);
            mBtnSignUp.Click += (object sender, EventArgs args) => 
            {
                //Se è una Dialog
                /*FragmentTransaction transaction = FragmentManager.BeginTransaction();
                Dialog_SignUp signUp = new Dialog_SignUp();
                signUp.Show(transaction, "Dialog fragment");*/
                //Se è una Activity
                Intent openPage2 = new Intent(this, typeof(SignUpActivity));
                this.StartActivity(openPage2);
            };
            /*mBtnSignIn.Click += delegate
            {
                
                Intent openPage1 = new Intent(this, typeof(ViewDatabase)); 
                this.StartActivity(openPage1);
            };*/

        }
    }
}


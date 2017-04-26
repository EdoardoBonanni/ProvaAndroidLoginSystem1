using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Java.Lang;
using ProvaAndroidLoginSystem1.Resources;
using Android.Content;
using ProvaAndroidLoginSystem1.Resources.Model;

namespace ProvaAndroidLoginSystem1
{
    [Activity(Label = "", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static HTTPClient client;
        private Button mBtnSignUp;
        private ProgressBar mProgressBar;
        private Button mBtnSignIn;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            client = new HTTPClient();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);

            mBtnSignUp.Click += mBtnSignUp_Click;
            mBtnSignIn.Click += mbtnSignIn_Click;
        }
        
        void mBtnSignUp_Click(object sender, EventArgs e)
        {
            Intent SignUp = new Intent(this, typeof(SignUpActivity));
            this.StartActivity(SignUp);
            this.OverridePendingTransition(Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

        }

        void mbtnSignIn_Click(object sender, EventArgs e)
        {
            Intent SignIn = new Intent(this, typeof(SignInActivity));
            this.StartActivity(SignIn);
        }
    }
 }


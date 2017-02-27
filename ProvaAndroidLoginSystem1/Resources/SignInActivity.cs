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
using ProvaAndroidLoginSystem1.Resources.DataHelper;
using Android.Views.InputMethods;
using ProvaAndroidLoginSystem1.Resources.Model;
using Android.Util;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "SignInActivity")]
    class SignInActivity : Activity
    {
        private DataBase db;
        private InputMethodManager imm;
        private TextView mtxtEmail;
        private TextView mtxtPassword;
        private Button mbtnSignIn;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SignInLayout);

            db = new DataBase();
            db.createDataBase();

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            
            mtxtEmail = FindViewById<EditText>(Resource.Id.txtEmailSignIn);
            mtxtPassword = FindViewById<EditText>(Resource.Id.txtPasswordSignIn);
            mbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);

            mbtnSignIn.Click += mbtnSignIn_Click;
        }

        void mbtnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                Person person = new Person()
                {
                    Email = mtxtEmail.Text,
                    Password = mtxtPassword.Text
                };
                imm.HideSoftInputFromWindow(mtxtEmail.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtPassword.WindowToken, 0);

                if (db.Login(person))
                {
                    Toast.MakeText(this, "Login Successful", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Login Failed", ToastLength.Long).Show();
                }
            }
            catch (Exception ex) { }
        }
    }
}
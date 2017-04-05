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
        private TextView mtxtNickname;
        private TextView mtxtPassword;
        private Button mbtnSignIn;
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SignInLayout);

            db = new DataBase();
            db.createDataBase();

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            
            mtxtNickname = FindViewById<EditText>(Resource.Id.txtNicknameSignIn);
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
                    Nickname = mtxtNickname.Text,
                    Password = CreateMD5(mtxtPassword.Text)
                };
                imm.HideSoftInputFromWindow(mtxtNickname.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtPassword.WindowToken, 0);

                if (db.Login(person))
                {
                    Toast.MakeText(this, "Login Successful", ToastLength.Long).Show();
                    Intent Home = new Intent(this, typeof(HomeActivity));
                    this.StartActivity(Home);
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
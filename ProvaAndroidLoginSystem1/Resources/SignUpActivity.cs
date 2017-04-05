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
using ProvaAndroidLoginSystem1.Resources.Model;
using Android.Views.InputMethods;

namespace ProvaAndroidLoginSystem1.Resources
{
    public class OnSignUpEventArgs : EventArgs
    {
        public string mFirstName { get; set; }
        public string mNickname { get; set; }
        public string mPassword { get; set; }
        public OnSignUpEventArgs(string firstname, string nickname, string password) : base()
        {
            mFirstName = firstname;
            mNickname= nickname;
            mPassword = password;
        }
    }

    [Activity(Label = "SignUp")]
    class SignUpActivity : Activity
    {
        private TextView mtxtFirstName;
        private TextView mtxtNickname;
        private TextView mtxtPassword;
        private Button mbtnSignUp;
        private Button mbtnDatabase;
        private DataBase db;
        private InputMethodManager imm;

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
            SetContentView(Resource.Layout.dialog_sign_up);

            db = new DataBase();
            db.createDataBase();

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);

            mtxtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
            mtxtNickname = FindViewById<EditText>(Resource.Id.txtNickname);
            mtxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mbtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mbtnDatabase = FindViewById<Button>(Resource.Id.btnDatabase);

            mbtnSignUp.Click += mbtnSignUp_Click;
            mbtnDatabase.Click += (object sender, EventArgs args) =>
            {
                ViewDatabase viewdatabase = new ViewDatabase();
                Intent Database = new Intent(this, typeof(ViewDatabase));
                this.StartActivity(Database);
            };
        }

        void mbtnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                //onSignUpComplete.Invoke(this, new OnSignUpEventArgs(mtxtFirstName.Text, mtxtEmail.Text, mtxtPassword.Text));
                Person person = new Person()
                {
                    Firstname = mtxtFirstName.Text,
                    Nickname = mtxtNickname.Text,
                    Password = CreateMD5(mtxtPassword.Text)
                };

                imm.HideSoftInputFromWindow(mtxtFirstName.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtNickname.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtPassword.WindowToken, 0);

                if (!db.InsertIntoTable(person))
                {
                    Toast.MakeText(this, "Nickname già utilizzato", ToastLength.Long).Show();
                }
                else
                {
                    mtxtFirstName.Text = "";
                    mtxtNickname.Text = "";
                    mtxtPassword.Text = "";
                }
            }
            catch (Exception ex) { }
        }
    }
}
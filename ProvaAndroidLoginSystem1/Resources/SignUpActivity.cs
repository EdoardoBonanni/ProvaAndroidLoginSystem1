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

namespace ProvaAndroidLoginSystem1.Resources
{
    public class OnSignUpEventArgs : EventArgs
    {
        public string mFirstName { get; set; }
        public string mEmail { get; set; }
        public string mPassword { get; set; }
        public OnSignUpEventArgs(string firstname, string email, string password) : base()
        {
            mFirstName = firstname;
            mEmail = email;
            mPassword = password;
        }
    }

    [Activity(Label = "ViewDatabase")]
    class SignUpActivity : Activity
    {
        private TextView mtxtFirstName;
        private TextView mtxtEmail;
        private TextView mtxtPassword;
        private Button mbtnSignUp;
        private Button mbtnDatabase;
        private DataBase db;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.dialog_sign_up);
            db = new DataBase();
            db.createDataBase();
            mtxtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
            mtxtEmail = FindViewById<EditText>(Resource.Id.txtEmail);
            mtxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mbtnSignUp = FindViewById<Button>(Resource.Id.btnDialogEmail);
            mbtnDatabase = FindViewById<Button>(Resource.Id.btnDatabase);
            mbtnSignUp.Click += mbtnSignUp_Click;
            mbtnDatabase.Click += (object sender, EventArgs args) =>
            {
                //ViewDatabase viewdatabase = new ViewDatabase();
                Intent openPage1 = new Intent(this, typeof(ViewDatabase));
                this.StartActivity(openPage1);

            };
        }

        void mbtnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                //onSignUpComplete.Invoke(this, new OnSignUpEventArgs(mtxtFirstName.Text, mtxtEmail.Text, mtxtPassword.Text));
                mbtnSignUp.Click += delegate
                {
                    Person person = new Person()
                    {
                        Firstname = mtxtFirstName.Text,
                        Email = mtxtEmail.Text,
                        Password = mtxtPassword.Text
                    };
                    db.InsertIntoTable(person);
                };
            }
            catch (Exception ex) { }
        }
    }
}
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
using Android.Database.Sqlite;
using ProvaAndroidLoginSystem1.Resources.DataHelper;
using ProvaAndroidLoginSystem1.Resources.Model;

namespace ProvaAndroidLoginSystem1
{
    public class OnSignUpEventArgs: EventArgs
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
    class Dialog_SignUp : DialogFragment
    {
        private TextView mtxtFirstName;
        private TextView mtxtEmail;
        private TextView mtxtPassword;
        private Button mbtnSignUp;
        DataBase db = new DataBase();
        private ListView mListview;
        private List<Person> ListPerson;
        public event EventHandler<OnSignUpEventArgs> onSignUpComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);
            mtxtFirstName = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            mtxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            mtxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mbtnSignUp = view.FindViewById<Button>(Resource.Id.btnSignUp);
            //db.createDataBase();

            return view;
        }

        void mbtnSignUp_Click(object sender, EventArgs e)
        {
            onSignUpComplete.Invoke(this, new OnSignUpEventArgs(mtxtFirstName.Text, mtxtEmail.Text, mtxtPassword.Text));
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

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

        private void LoadData()
        {
            ListPerson = db.selectTable();
            /*var adapter = new ListViewAdapter(this, )*/
        }
    }
}
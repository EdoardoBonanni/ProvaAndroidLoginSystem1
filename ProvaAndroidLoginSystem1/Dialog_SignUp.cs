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
using ProvaAndroidLoginSystem1.Resources;

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
    public class Dialog_SignUp : DialogFragment
    {
        private TextView mtxtFirstName;
        private TextView mtxtEmail;
        private TextView mtxtPassword;
        private Button mbtnSignUp;
        private Button mbtnDatabase;
        private DataBase db;
        private ListView mListview;
        private List<Person> ListPerson;

        public event EventHandler<OnSignUpEventArgs> onSignUpComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            
            db = new DataBase();
            db.createDataBase();
            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);
            mtxtFirstName = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            mtxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            mtxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            mbtnSignUp = view.FindViewById<Button>(Resource.Id.btnSignUp);
            mbtnDatabase = view.FindViewById<Button>(Resource.Id.btnDatabase);
            /*mListview.ItemClick += (s, e) => {
                for (int i = 0; i < mListview.Count; i++)
                {
                    if (e.Position == i)
                        mListview.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.DarkGray);
                    else
                        mListview.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                }
                
                var txtname = e.View.FindViewById<TextView>(Resource.Id.textView1);
                var txtemail = e.View.FindViewById<TextView>(Resource.Id.textView2);
                var txtpassword = e.View.FindViewById<TextView>(Resource.Id.textView3);
            };*/
            mbtnSignUp.Click += mbtnSignUp_Click;
            mbtnDatabase.Click += (object sender, EventArgs args) =>
            {
                ViewDatabase viewdatabase = new ViewDatabase();
                var myIntent = new Intent(Context, typeof(ViewDatabase));
                StartActivityForResult(myIntent, 0);
                /*viewdatabase.StartActivity(typeof(Activity));*/

            };
            return view;
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
                        Nickname = mtxtEmail.Text,
                        Password = mtxtPassword.Text
                    };
                    db.InsertIntoTable(person);
                };
            }
            catch(Exception ex) { }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }

        
    }
}
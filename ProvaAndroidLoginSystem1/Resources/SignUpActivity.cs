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
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

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
        private HTTPClient client;
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

            client = new HTTPClient();

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

        public async Task<HttpResponseMessage> RegisterAsync(Person person)
        {
            var uri = new Uri("http://mobileapi-edobona98.c9users.io/Login/register.php");
            var json = JsonConvert.SerializeObject(person);
            /*var content = new StringContent(@"{""Firstname"" : ""ciao"", ""Nickname"" : ""ciao"", ""Password"" : ""ciao""}", Encoding.UTF8, "application/json");*/

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Firstname", person.Firstname),
                new KeyValuePair<string, string>("Nickname", person.Nickname),
                new KeyValuePair<string, string>("Password", person.Password)
            });

            HttpResponseMessage response = null;
            try
            {
                response = await client.Client.PostAsync(uri, content);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.InnerException);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        async void mbtnSignUp_Click(object sender, EventArgs e)
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

                var response = await RegisterAsync(person);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var jwt = JObject.Parse(result)["JWT"].ToString();
                }
                else
                {
                    var error = JObject.Parse(result)["error"].ToString();
                    Toast.MakeText(this, error, ToastLength.Long).Show();
                }
                /*
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
                */
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}
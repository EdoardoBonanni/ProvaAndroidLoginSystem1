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
using ProvaAndroidLoginSystem1.Resources.Model;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using p2p_project;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "SignUp")]
    class SignUpActivity : Activity
    {
        private EditText mtxtFirstName;
        private EditText mtxtNickname;
        private EditText mtxtPassword;
        private Button mbtnSignUp;
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

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.dialog_sign_up);

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);

            mtxtFirstName = FindViewById<EditText>(Resource.Id.txtFirstName);
            mtxtNickname = FindViewById<EditText>(Resource.Id.txtNickname);
            mtxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mbtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);

            mbtnSignUp.Click += mbtnSignUp_Click;

            base.OnCreate(bundle);
        }

        public async Task<HttpResponseMessage> RegisterAsync(Person person)
        {
            var uri = new Uri("http://mobileapi-edobona98.c9users.io/utente");
            var json = JsonConvert.SerializeObject(person);
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("method", "register"),
                new KeyValuePair<string, string>("Firstname", person.Firstname),
                new KeyValuePair<string, string>("Nickname", person.Nickname),
                new KeyValuePair<string, string>("Password", person.Password)
            });

            HttpResponseMessage response = null;
            try
            {
                response = await MainActivity.client.Client.PostAsync(uri, content);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        async void mbtnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                if(mtxtFirstName.Text.Equals("") || this.mtxtNickname.Text.Equals("") || this.mtxtPassword.Text.Equals(""))
                {
                    Toast.MakeText(this, "All the textboxs must be filled", ToastLength.Long).Show();
                    return;
                }
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
                    Intent Home = new Intent(this, typeof(HomeActivity));
                    this.StartActivity(Home);
                }
                else
                {
                    var error = JObject.Parse(result)["error"].ToString();
                    Toast.MakeText(this, error, ToastLength.Long).Show();
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}
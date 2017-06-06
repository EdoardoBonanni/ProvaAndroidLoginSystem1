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
using Android.Views.InputMethods;
using ProvaAndroidLoginSystem1.Resources.Model;
using Android.Util;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using p2p_project;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "SignInActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class SignInActivity : Activity
    {
        private InputMethodManager imm;
        private EditText mtxtUsername;
        private EditText mtxtPassword;
        private Button mbtnSignIn;
        private bool isBackup;

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

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            
            mtxtUsername = FindViewById<EditText>(Resource.Id.txtNicknameSignIn);
            mtxtPassword = FindViewById<EditText>(Resource.Id.txtPasswordSignIn);
            mbtnSignIn = FindViewById<Button>(Resource.Id.btnSignIn);

            mbtnSignIn.Click += mbtnSignIn_Click;

            isBackup = Intent.GetBooleanExtra("Backup", false);
        }

        async Task<HttpResponseMessage> RegisterAsync(Person person)
        {
            var uri = new Uri("http://mobileapi-edobona98.c9users.io/utente");
            var json = JsonConvert.SerializeObject(person);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("method", "login"),
                new KeyValuePair<string, string>("Username", person.Username),
                new KeyValuePair<string, string>("Password", person.Password)
            });

            HttpResponseMessage response = null;
            try
            {
                response = await MainActivity.client.Client.PostAsync(uri, content);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        async void mbtnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                Person person = new Person()
                {
                    Username = mtxtUsername.Text,
                    Password = CreateMD5(mtxtPassword.Text)
                };

                imm.HideSoftInputFromWindow(mtxtUsername.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtPassword.WindowToken, 0);

                var response = await RegisterAsync(person);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    MainActivity.saveLocal("Username", person.Username);
                    if (isBackup)
                    {
                        //Push on server
                    }
                    Intent Main = new Intent(this, typeof(MainActivity));
                    this.StartActivity(Main);
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
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
    [Activity(Label = "SignUp", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class SignUpActivity : Activity
    {
        private EditText mtxtUsername;
        private EditText mtxtPassword;
        private EditText mtxtConfirmPassword;
        private Button mbtnSignUp;
        private TextView mtxtGoToSignIn;
        private InputMethodManager imm;
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

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.dialog_sign_up);

            imm = (InputMethodManager)GetSystemService(Context.InputMethodService);

            mtxtUsername = FindViewById<EditText>(Resource.Id.txtNickname);
            mtxtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            mtxtConfirmPassword = FindViewById<EditText>(Resource.Id.txtConfermaPassword);
            mbtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mtxtGoToSignIn = FindViewById<TextView>(Resource.Id.txtGoToSignIn);

            mbtnSignUp.Click += mbtnSignUp_Click;
            mtxtGoToSignIn.Click += mTxtGoToSignIn_Click;

            isBackup = Intent.GetBooleanExtra("Backup", false);

            base.OnCreate(bundle);
        }

        public async Task<HttpResponseMessage> RegisterAsync(Person person)
        {
            var uri = new Uri("http://mobileapi-edobona98.c9users.io/utente");
            var json = JsonConvert.SerializeObject(person);
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("method", "register"),
                new KeyValuePair<string, string>("Username", person.Username),
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
                if(this.mtxtUsername.Text.Equals("") || this.mtxtPassword.Text.Equals("") || this.mtxtConfirmPassword.Equals(""))
                {
                    Toast.MakeText(this, "Devi completare tutti i campi", ToastLength.Long).Show();
                    return;
                }
                else if (!mtxtPassword.Text.Equals(mtxtConfirmPassword.Text))
                {
                    Toast.MakeText(this, "Il campo Conferma password deve essere uguale al campo Password", ToastLength.Long).Show();
                    return;
                }

                Person person = new Person()
                {
                    Username = mtxtUsername.Text,
                    Password = CreateMD5(mtxtPassword.Text)
                };

                imm.HideSoftInputFromWindow(mtxtUsername.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtPassword.WindowToken, 0);
                imm.HideSoftInputFromWindow(mtxtConfirmPassword.WindowToken, 0);

                var response = await RegisterAsync(person);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    MainActivity.saveLocal("Username", person.Username);
                    if (isBackup)
                    {
                        //Push on server
                    }
                    Intent main = new Intent(this, typeof(MainActivity));
                    this.StartActivity(main);
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

        private void mTxtGoToSignIn_Click(object sender, EventArgs e)
        {
            Intent SignIn = new Intent(this, typeof(SignInActivity));
            if (isBackup)
            {
                SignIn.PutExtra("Backup", true);
            }
            this.StartActivity(SignIn);
        }
    }
}
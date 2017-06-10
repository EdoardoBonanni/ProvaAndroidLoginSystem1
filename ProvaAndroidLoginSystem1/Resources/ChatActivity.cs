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
using p2p_project;
using p2p_project.Resources;
using p2p_project.Resources.DataHelper;
using p2p_project.Resources.Model;
using Android.Telephony;
using Newtonsoft.Json;
using Java.Lang;
using Android.Content.Res;
using Java.IO;
using Android.Provider;
using Android.Graphics;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "Chat P2P", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class ChatActivity : Activity
    {
        private ListView lstMessage;
        private Button btnFile;
        private Button btnSend;
        private EditText txtChat;


        private ChatAdapter chatAdapter;
        private Database database;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChatLayout);

            lstMessage = FindViewById<ListView>(Resource.Id.lstMessages);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            btnFile = FindViewById<Button>(Resource.Id.btnSendFile);
            txtChat = FindViewById<EditText>(Resource.Id.txtChat);

            btnSend.Click += btnSend_Click;
            btnFile.Click += BtnFile_Click;
            lstMessage.ItemClick += LstMessage_ItemClick;

            database = new Database();
            database.createTable();

            Database.databaseUpdated += (sender, args, reg, mine) =>
            {
                RunOnUiThread(() =>
                {
                    updateChat(reg.Messaggio, mine, reg.isFile, reg.Path);
                });
            };

            PacketManager.partFileReceived += PacketManager_partFileReceived;

            List<Tuple<Registro, bool>> chat = new List<Tuple<Registro, bool>>();
            
            List<Registro> registro = new List<Registro>();
            registro = database.SelectQueryTable(MainActivity.retrieveLocal("Username"), MainActivity.retrieveLocal("ConnectedUsername"), 1);

            foreach (var message in registro)
            {
                if (message.UsernameMittente.Equals(MainActivity.retrieveLocal("Username")))
                {
                    chat.Add(new Tuple<Registro, bool>(message, true));
                }
                else
                {
                    chat.Add(new Tuple<Registro, bool>(message, false));
                }
            }
            /*new AlertDialog.Builder(this)
                .SetPositiveButton("OK", delegate { })
                .SetMessage("E' attiva la modalità anonima.\nI dati non verranno salvati nel Database.")
                .SetTitle("Warning")
                .Show();*/

                //alert.SetView(LayoutInflater.Inflate(Resource.Layout.dialog_sign_up, null));

            chatAdapter = new ChatAdapter(this, chat);
            lstMessage.Adapter = chatAdapter;
        }

        private void PacketManager_partFileReceived(object sender, EventArgs e, string fileName, string path, bool mine)
        {
            if (chatAdapter.test(path))
            {
                RunOnUiThread(() =>
                {
                    updateChat(fileName + " (in corso...)", mine, true, path);
                });
            }
        }

        private void LstMessage_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Registro itemClicked = (Registro) chatAdapter.GetItem(e.Position);
            if (itemClicked.isFile)
            {
                if(!itemClicked.Messaggio.Contains("(in corso...)"))
                {
                    File f = new File(itemClicked.Path);
                    string uri = Android.Net.Uri.FromFile(f).ToString();
                    Intent SendFileNow = new Intent(this, typeof(SendFileActivity));

                    var gallery = new
                    {
                        GetFrom = "Received",
                        Uri = uri, 
                        Path = itemClicked.Path
                    };
                    string obj = JsonConvert.SerializeObject(gallery);
                    SendFileNow.PutExtra("SelectFile", obj);
                    this.StartActivity(SendFileNow);
                }
            }
        }

        private void BtnFile_Click(object sender, EventArgs e)
        {
            Intent SelectFile = new Intent(this, typeof(SelectFileActivity));
            this.StartActivity(SelectFile);
        }

        public void updateChat(string text, bool mine, bool isFile, string path = "")
        {
            if (mine && !isFile)
            {
                database.InsertIntoTable(new Registro
                {
                    UsernameMittente = mine == true ? MainActivity.retrieveLocal("Username") : MainActivity.retrieveLocal("ConnectedUsername"),
                    UsernameDestinatario = mine == false ? MainActivity.retrieveLocal("Username") : MainActivity.retrieveLocal("ConnectedUsername"),
                    isFile = false,
                    Messaggio = text,
                    Path = "",
                    Orario = DateTime.Now
                });
            }

            chatAdapter.update(text, mine, path, isFile);
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            if (!txtChat.Text.Equals(""))
            {
                ISocket socket = ConnectionInfoListener.Socket;

                string packet = PacketManager.PackMessage(txtChat.Text);
                socket.Send(packet);

                updateChat(txtChat.Text, true, false);

                txtChat.Text = "";
            }
        }

        public override void OnBackPressed()
        {
            Intent main = new Intent(this, typeof(MainActivity));
            this.StartActivity(main);
        }
    }
}
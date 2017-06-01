﻿using System;
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

            //Lo farà il background Service
            PacketManager.messageReceived += (sender, args, message) =>
            {
                RunOnUiThread(() =>
                {
                    updateChat(message, false, false);
                });
            };

            /*
             * Lo fa il background Service
            PacketManager.fileReceived += (sender, args, uri) =>
            {
                RunOnUiThread(() =>
                {
                    updateChat(uri, true, true);
                });
            };
            */

            database = new Database();
            database.createTable();

            /*
            database.databaseUpdated += (sender, args, reg) =>
            {
                RunOnUiThread(() =>
                {
                    updateChat(reg.Messaggio, true, reg.isFile);
                });
            };
            */

            List<Tuple<Registro, bool>> chat = new List<Tuple<Registro, bool>>();
            
            List<Registro> registro = new List<Registro>();
            registro = database.SelectQueryTable(MainActivity.retrieveLocal("Username"), MainActivity.retrieveLocal("ConnectedUsername"));

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

        private void LstMessage_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var itemClicked = sender as Tuple<Registro, bool>;
            if (itemClicked.Item1.isFile)
            {
                //Start Activity
                //PutExtra("Uri", itemClicked.Item1.Messaggio);
            }
        }

        private void BtnFile_Click(object sender, EventArgs e)
        {
            Intent SelectFile = new Intent(this, typeof(SelectFileActivity));
            this.StartActivity(SelectFile);
        }

        public void updateChat(string text, bool mine, bool isFile)
        {
            //Background Service
            database.InsertIntoTable(new Registro
            {
                UsernameMittente = mine == true ? MainActivity.retrieveLocal("Username") : MainActivity.retrieveLocal("ConnectedUsername"),
                UsernameDestinatario = mine == false ? MainActivity.retrieveLocal("Username") : MainActivity.retrieveLocal("ConnectedUsername"),
                isFile = false,
                Messaggio = text,
                Orario = DateTime.Now
            });

            chatAdapter.update(text, mine, isFile);
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            ISocket socket = ConnectionInfoListener.Socket;

            string packet = PacketManager.PackMessage(txtChat.Text);
            socket.Send(packet);

            updateChat(txtChat.Text, true, false);

            txtChat.Text = "";
        }
    }
}
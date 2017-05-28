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
    [Activity(Label = "Chat P2P")]
    public class ChatActivity : Activity
    {
        private ListView lstMessage;
        private Button btnSend;
        private EditText txtChat;
        /*private SocketServer server;
        private ClientSocket client;*/
        private ChatAdapter chatAdapter;
        private Database database;

        public static bool AnonymousMode { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChatLayout);

            lstMessage = FindViewById<ListView>(Resource.Id.lstMessages);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            txtChat = FindViewById<EditText>(Resource.Id.txtChat);

            btnSend.Click += btnSend_Click;

            PacketManager.messageReceived += (sender, args, message) =>
            {
                RunOnUiThread(() =>
                {
                    updateChat(message, false);
                });
            };

            database = new Database();
            database.createTable();

            List<Tuple<Registro, bool>> chat = new List<Tuple<Registro, bool>>();

            if (!MainActivity.Number.Equals("") && MainActivity.Number != null && MainActivity.retrievePhoneNumber("ConnectedPhoneNumber") != null)
            {
                List<Registro> registro = new List<Registro>();
                registro = database.SelectQueryTable(MainActivity.Number, MainActivity.retrievePhoneNumber("ConnectedPhoneNumber"));

                foreach (var message in registro)
                {
                    if (message.isFile)
                    {
                        break;
                    }

                    if (message.PhoneNumberMittente.Equals(MainActivity.Number))
                    {
                        chat.Add(new Tuple<Registro, bool>(message, true));
                    }
                    else
                    {
                        chat.Add(new Tuple<Registro, bool>(message, false));
                    }
                }
            }
            else
            {
                AnonymousMode = true;
                new AlertDialog.Builder(this)
                .SetPositiveButton("OK", (sender, args) => { })
                .SetMessage("E' attiva la modalità anonima.\nI dati non verranno salvati nel Database.")
                .SetTitle("Warning")
                .Show();

                //alert.SetView(LayoutInflater.Inflate(Resource.Layout.dialog_sign_up, null));
            }

            chatAdapter = new ChatAdapter(this, chat);
            lstMessage.Adapter = chatAdapter;
        }

        public void updateChat(string text, bool mine)
        {
            if (!AnonymousMode)
            {
                database.InsertIntoTable(new Registro
                {
                    PhoneNumberMittente = mine == true ? MainActivity.Number : MainActivity.retrievePhoneNumber("ConnectedPhoneNumber"),
                    PhoneNumberDestinatario = mine == false ? MainActivity.Number : MainActivity.retrievePhoneNumber("ConnectedPhoneNumber"),
                    isFile = false,
                    Messaggio = text,
                    Orario = DateTime.Now
                });
            }
            chatAdapter.update(text, mine);
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            string packet = JsonConvert.SerializeObject(new
            {
                Type = "Message",
                Buffer = txtChat.Text,
                Checksum = ""
            });
            if (ConnectionInfoListener.isServer)
            {
                SocketServer server = ConnectionInfoListener.Server;
                server.Send(packet);
            }
            else
            {
                ClientSocket client = ConnectionInfoListener.Client;
                client.Send(packet);
            }
            updateChat(txtChat.Text, true);
            txtChat.Text = "";
        }
    }
}
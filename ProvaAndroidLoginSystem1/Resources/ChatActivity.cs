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

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "Chat P2P")]
    public class ChatActivity : Activity
    {
        private ListView lstMessage;
        private Button btnSend;
        private EditText txtChat;
        private SocketServer server;
        private ClientSocket client;
        private ChatAdapter chatAdapter;
        private Database database;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ChatLayout);

            lstMessage = FindViewById<ListView>(Resource.Id.lstMessages);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            txtChat = FindViewById<EditText>(Resource.Id.txtChat);

            btnSend.Click += btnSend_Click;

            database = new Database();
            database.createTable();

            List<Registro> registro = database.SelectQueryTable(1, 2);
            List<Tuple<string, bool>> chat = new List<Tuple<string, bool>>();

            foreach (var message in registro)
            {

                if (message.isFile)
                {
                    break;
                }
                if(message.IdMittente == 1/*Id dell'utente*/)
                {
                    chat.Add(new Tuple<string, bool>(message.Messaggio, true));
                }
                else
                {
                    chat.Add(new Tuple<string, bool>(message.Messaggio, false));
                }
            }

            chatAdapter = new ChatAdapter(this, chat);
            lstMessage.Adapter = chatAdapter;
        }

        public void updateChat(string text, bool mine)
        {
            chatAdapter.update(text, mine);
            //inserimento nel Db
            database.InsertIntoTable(new Registro
            {
                IdMittente = mine == true ? 1 : 2/*IdProprietario : IdConnesso*/,
                IdDestinatario = mine == false ? 1 : 2,
                isFile = false,
                Messaggio = text,
                Orario = DateTime.Now
            });
        }

        void btnSend_Click(object sender, EventArgs e)
        {
            if (ConnectionInfoListener.isServer)
            {
                SocketServer server = ConnectionInfoListener.Server;
                server.Send(txtChat.Text);
            }
            else
            {
                ClientSocket client = ConnectionInfoListener.Client;
                client.Send(txtChat.Text);
            }
            updateChat(txtChat.Text, true);
            txtChat.Text = "";
        }
    }
}
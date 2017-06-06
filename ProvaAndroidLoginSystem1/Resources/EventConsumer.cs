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
using p2p_project.Resources.DataHelper;
using p2p_project.Resources.Model;
using ProvaAndroidLoginSystem1;

namespace p2p_project.Resources
{
    class EventConsumer
    {
        private Database database;

        public EventConsumer()
        {
            database = new Database();
            database.createTable();
            PacketManager.messageReceived += PacketManager_messageReceived;
            PacketManager.fileReceived += PacketManager_fileReceived;
        }

        private void PacketManager_fileReceived(object sender, EventArgs e, string fileName, string path, bool mine)
        {
            database.InsertIntoTable(new Registro
            {
                UsernameMittente = mine == true ? MainActivity.retrieveLocal("Username") : MainActivity.retrieveLocal("ConnectedUsername"),
                UsernameDestinatario = mine == false ? MainActivity.retrieveLocal("Username") : MainActivity.retrieveLocal("ConnectedUsername"),
                isFile = true,
                Messaggio = fileName,
                Path = path,
                Orario = DateTime.Now
            });
        }

        private void PacketManager_messageReceived(object sender, EventArgs e, string message)
        {
            database.InsertIntoTable(new Registro
            {
                UsernameMittente = MainActivity.retrieveLocal("ConnectedUsername"),
                UsernameDestinatario = MainActivity.retrieveLocal("Username"),
                isFile = false,
                Messaggio = message,
                Path = "",
                Orario = DateTime.Now
            });
        }
    }
}
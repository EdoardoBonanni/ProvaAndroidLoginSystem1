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
using Newtonsoft.Json;
using ProvaAndroidLoginSystem1;
using ProvaAndroidLoginSystem1.Resources;

namespace p2p_project.Resources
{
    public delegate void MessageEventHandler(object sender, EventArgs e, string message);
    public delegate void UsernameEventHandler(object sender, EventArgs e);

    class PacketManager
    {
        public static event MessageEventHandler messageReceived;
        public static event UsernameEventHandler usernameReceived;

        protected virtual void OnMessageReceived(EventArgs e, string message)
        {
            messageReceived?.Invoke(this, e, message);
        }

        protected virtual void OnUsernameReceived(EventArgs e)
        {
            usernameReceived?.Invoke(this, e);
        }

        public object Unpack(string packet)
        {
            dynamic a = JsonConvert.DeserializeObject<dynamic>(packet);
            string type = a.Type;
            switch (type)
            {
                case "Username":
                    if(a.Buffer != "")
                    {
                        string Username = a.Buffer;
                        MainActivity.saveLocal("ConnectedUsername", Username);
                    }
                    OnUsernameReceived(EventArgs.Empty);
                    break;
                case "Message":
                    string message = a.Buffer;
                    OnMessageReceived(EventArgs.Empty, message);
                    break;
                case "File":
                    break;
                default:
                    //Errore
                    break;
            }
            return null;
        }

        public static string PackUsername(string username)
        {
            return JsonConvert.SerializeObject(new
                {
                    Type = "Username",
                    Buffer = MainActivity.retrieveLocal("Username") ?? "",
                    Checksum = ""
                });
        }

        public static string PackMessage(string message)
        {
            return JsonConvert.SerializeObject(new
                {
                    Type = "Message",
                    Buffer = message,
                    Checksum = ""
                });
        }

        public static string PackFile()
        {
            return "";
        }
    }
}
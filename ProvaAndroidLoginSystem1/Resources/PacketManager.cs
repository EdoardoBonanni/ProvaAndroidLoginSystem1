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
    public delegate void PhoneNumberEventHandler(object sender, EventArgs e);

    class PacketManager
    {
        public static event MessageEventHandler messageReceived;
        public static event PhoneNumberEventHandler phoneNumberReceived;

        protected virtual void OnMessageReceived(EventArgs e, string message)
        {
            messageReceived?.Invoke(this, e, message);
        }

        protected virtual void OnPhoneNumberReceived(EventArgs e)
        {
            phoneNumberReceived?.Invoke(this, e);
        }

        public object Unpack(string packet)
        {
            dynamic a = JsonConvert.DeserializeObject<dynamic>(packet);
            string type = a.Type;
            switch (type)
            {
                case "PhoneNumber":
                    if(a.Buffer != "")
                    {
                        string phoneNumber = a.Buffer;
                        MainActivity.savePhoneNumber("ConnectedPhoneNumber", phoneNumber);
                    }
                    OnPhoneNumberReceived(EventArgs.Empty);
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

        public void PackNumber(string phoneNumber)
        {

        }

        public void PackMessage(string message)
        {

        }

        public void PackFile()
        {

        }
    }
}
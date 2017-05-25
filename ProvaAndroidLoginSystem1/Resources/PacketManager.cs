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

namespace p2p_project.Resources
{
    class PacketManager
    {
        public object Unpack(string packet)
        {
            dynamic a = JsonConvert.DeserializeObject<dynamic>(packet);
            string type = a.Type;
            switch (type)
            {
                case "PhoneNumber":
                    //MainActivity.saveId("ConnectedPhoneNumber", a.Buffer);
                    Console.WriteLine("Il numero di telefono del peer connesso è: " + a.Buffer);
                    break;
                case "Message":
                    //Inserimento nel Db
                    Console.WriteLine("Il messaggio ricevuto è: " + a.Buffer);
                    break;
                case "File":
                    break;
                case "Anonymous":
                    //Setta variabile boolean a true, accessibile da ovunque
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
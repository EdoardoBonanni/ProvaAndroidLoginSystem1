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
using SQLite;

namespace p2p_project.Resources.Model
{
    class Registro
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UsernameMittente { get; set; }

        public string UsernameDestinatario { get; set; }

        public bool isFile { get; set; }

        public string Messaggio { get; set; }

        public DateTime Orario { get; set; }
    }
}
using System;
using SQLite;

namespace p2p_project.Resources.Model
{
    class Registro : Java.Lang.Object
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UsernameMittente { get; set; }

        public string UsernameDestinatario { get; set; }

        public bool isFile { get; set; }

        public string Messaggio { get; set; }

        public DateTime Orario { get; set; }

        public string Path { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using p2p_project.Resources.Model;
using ProvaAndroidLoginSystem1;

namespace p2p_project.Resources.DataHelper
{
    delegate void DatabaseEventHandler(object sender, EventArgs e, Registro registro, bool mine);
    class Database
    {
        public static int count = 15;
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static event DatabaseEventHandler databaseUpdated;
        private void OnDatabaseUpdated(EventArgs e, Registro registro, bool mine)
        {
            databaseUpdated?.Invoke(this, e, registro, mine);
        }

        public bool createTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    //controllare se esiste la tabella
                    var test = connection.Query<int>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name =?", "Registro");
                    if (test[0] == 0)
                    {
                        connection.CreateTable<Registro>();
                    }
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTable(Registro registro)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    connection.Insert(registro);
                    bool mine = registro.UsernameMittente == MainActivity.retrieveLocal("Username");
                    if (!mine || (mine && registro.isFile))
                    {
                        OnDatabaseUpdated(EventArgs.Empty, registro, mine);
                    }
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public List<Registro> selectTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    return connection.Table<Registro>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return null;
            }
        }

        /*public bool UpdateRowTable(Registro registro)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    //connection.Query<Registro>("UPDATE Person set Firstname=?, Nickname=?, Password=? Where Id=?", person.Firstname, person.Nickname, person.Password, person.Id);
                    connection.Query<Registro>("UPDATE Registro ");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }*/

        /*public bool DeleteRowTable(Registro registro)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    connection.Delete(registro);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }*/

        public bool DeleteAllRowTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    connection.DeleteAll<Registro>();
                    connection.DropTable<Registro>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public List<Registro> SelectQueryTable(string Propietario, string Connesso, int offset)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    List<Registro> chat = connection.Query<Registro>("Select * FROM Registro WHERE (UsernameMittente=? AND UsernameDestinatario=?) OR (UsernameMittente=? AND UsernameDestinatario=?) ORDER BY orario DESC LIMIT " + offset + ", " + count + ";", Propietario, Connesso, Connesso, Propietario);
                    return chat;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return new List<Registro>();
            }
        }

    }
}
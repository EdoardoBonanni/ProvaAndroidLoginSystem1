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
using SQLite;
using ProvaAndroidLoginSystem1.Resources.Model;
using p2p_project.Resources.Model;

namespace p2p_project.Resources.DataHelper
{
    class Database
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public Database()
        {

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
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public List<Registro> SelectQueryTable(int Propietario, int Connesso)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "ChatP2p.db")))
                {
                    List<Registro> mittente = connection.Query<Registro>("Select * From Registro Where IdMittente=? AND IdDestinatario=?", Propietario, Connesso);
                    List<Registro> destinatario = connection.Query<Registro>("Select * From Registro Where IdMittente=? AND IdDestinatario=?", Connesso, Propietario);
                    return mittente.Concat<Registro>(destinatario).ToList();
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteException", ex.Message);
                return null;
            }
        }
    }
}
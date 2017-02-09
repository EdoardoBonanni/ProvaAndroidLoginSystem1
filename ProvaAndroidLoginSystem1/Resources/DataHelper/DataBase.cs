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
using Android.Util;
using ProvaAndroidLoginSystem1.Resources.Model;

namespace ProvaAndroidLoginSystem1.Resources.DataHelper
{
    public class DataBase
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool createDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.CreateTable<Person>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTable(Person person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Insert(person);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public List<Person> selectTable()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    return connection.Table<Person>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return null;
            }
        }

        public bool UpdateTable(Person person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Query<Person>("UPDATE Person set Firstname=?, Email=?, Password=? Where Id=?", person.Firstname, person.Email, person.Password, person.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public bool DeleteTable(Person person)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Delete(person);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

        public bool SelectQueryTable(int Id)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Persons.db")))
                {
                    connection.Query<Person>("Select * From Person Where Id=?", Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteException", ex.Message);
                return false;
            }
        }

    }
    
}
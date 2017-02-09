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

namespace ProvaAndroidLoginSystem1.Resources.Model
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
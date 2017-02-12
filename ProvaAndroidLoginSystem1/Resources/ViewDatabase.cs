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
using ProvaAndroidLoginSystem1.Resources.Model;
using ProvaAndroidLoginSystem1.Resources.DataHelper;

namespace ProvaAndroidLoginSystem1.Resources
{
    [Activity(Label = "ViewDatabase")]
    public class ViewDatabase : Activity
    {
        public ListView listview1;
        private List<Person> ListPerson;
        private DataBase db;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ListViewLayout);
            db = new DataBase();
            LoadData();
            listview1 = FindViewById<ListView>(Resource.Id.listView1);

        }

        private void LoadData()
        {
            try
            {
                ListPerson = db.selectTable();
                var adapter = new ListViewAdapter(this, ListPerson);
                listview1.Adapter = adapter;
            }
            catch (Exception ex) { }
        }
    }
}
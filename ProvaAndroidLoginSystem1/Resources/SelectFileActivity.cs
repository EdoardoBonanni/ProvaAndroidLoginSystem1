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
using Android.Provider;
using Android.Content.PM;
using Newtonsoft.Json;

namespace p2p_project.Resources
{
    [Activity(Label = "Seleziona File")]
    public class SelectFileActivity : Activity
    {
        private Button btnSendFileGallery;
        private Button btnSendFileNow;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SelectFileLayout);

            btnSendFileGallery = FindViewById<Button>(Resource.Id.btnSendFileGallery);
            btnSendFileNow = FindViewById<Button>(Resource.Id.btnSendFileNow);

            btnSendFileGallery.Click += btnSendFileGallery_Click;
            btnSendFileNow.Click += btnSendFileNow_Click;
        }

        private void btnSendFileNow_Click(object sender, EventArgs e)
        {
            if (IsThereAnAppToTakePictures())
            {
                Intent SendFileNow = new Intent(this, typeof(SendFileActivity));
                this.StartActivity(SendFileNow);
            }
            else
            {
                //avvertire l'utente
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void btnSendFileGallery_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                Intent SendFileNow = new Intent(this, typeof(SendFileActivity));
                
                var test = new { FromGallery = true, Uri = data.Data.ToString() };
                string obj = JsonConvert.SerializeObject(test);
                SendFileNow.PutExtra("SelectFile", obj);
                this.StartActivity(SendFileNow);
            }
        }
    }
}
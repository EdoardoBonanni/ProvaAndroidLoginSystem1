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
    [Activity(Label = "SendFileNow")]
    public class SendFileActivity : Activity
    {
        private ImageView imgPhoto;
        private Button btnSendFile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SendFileLayout);

            imgPhoto = FindViewById<ImageView>(Resource.Id.imageViewSendFile);

            var prova = Intent.GetStringExtra("SelectFile");
            var test = JsonConvert.DeserializeObject<dynamic>(prova);

            bool fromGallery = test.FromGallery;
            if (fromGallery)
            {
                string uri = test.Uri;
                imgPhoto.SetImageURI(Android.Net.Uri.Parse(uri));
            }
            else{

            }
        }
    }
}
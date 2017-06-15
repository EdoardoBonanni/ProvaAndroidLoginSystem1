using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Provider;
using Android.Content.PM;
using Newtonsoft.Json;
using Java.IO;
using Android.Graphics;
using Android.Database;

namespace p2p_project.Resources
{
    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }

    [Activity(Label = "Seleziona File", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SelectFileActivity : Activity
    {
        private Button btnSendFileGallery;
        private Button btnSendFileNow;
        private readonly int FROM_GALLERY = 1;
        private readonly int CAMERA = 2;

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
                CreateDirectoryForPictures("ChatP2p Fotocamera");

                Intent intent = new Intent(MediaStore.ActionImageCapture);
                App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));                
                StartActivityForResult(intent, CAMERA);
            }
            else
            {
                //avvertire l'utente
            }
        }

        private void CreateDirectoryForPictures(string dirName)
        {
            string path = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures).Path, "ChatP2p");
            App._dir = new Java.IO.File(path, dirName);
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
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
            imageIntent.SetAction(Intent.ActionPick);
            StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), FROM_GALLERY);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == FROM_GALLERY)
                {
                    Intent SendFileNow = new Intent(this, typeof(SendFileActivity));

                    var gallery = new
                    {
                        GetFrom = "Gallery",
                        Uri = data.Data.ToString(),
                        Path = GetPathToImage(data.Data)
                    };
                    string obj = JsonConvert.SerializeObject(gallery);
                    SendFileNow.PutExtra("SelectFile", obj);
                    this.StartActivity(SendFileNow);
                }
                else if (requestCode == CAMERA)
                {
                    // Make it available in the gallery
                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);

                    Intent SendFileNow = new Intent(this, typeof(SendFileActivity));

                    var camera = new
                    {
                        GetFrom = "Camera",
                        Uri = contentUri.ToString(),
                        Path = App._file.Path
                    };
                    string obj = JsonConvert.SerializeObject(camera);

                    SendFileNow.PutExtra("SelectFile", obj);
                    this.StartActivity(SendFileNow);
                }
            }
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            ICursor cursor = this.ContentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string document_id = cursor.GetString(0);
            //document_id = document_id.Split(':')[1];
            cursor.Close();

            cursor = ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }

    }
}
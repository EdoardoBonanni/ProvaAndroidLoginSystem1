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
using Android.Graphics;
using ProvaAndroidLoginSystem1.Resources;
using Android.Database;
using Android.Provider;

namespace p2p_project.Resources
{
    public delegate void SendFileEventHandler(object sender, EventArgs e, string uri, string path);

    [Activity(Label = "ChatP2p", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SendFileActivity : Activity
    {
        private ImageView imgPhoto;
        private Button btnSendFile;

        private Android.Net.Uri uri;
        private string path;

        public static event SendFileEventHandler firstSendFile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SendFileLayout);

            imgPhoto = FindViewById<ImageView>(Resource.Id.imageViewSendFile);
            btnSendFile = FindViewById<Button>(Resource.Id.btnSendFile);

            var prova = Intent.GetStringExtra("SelectFile");
            var test = JsonConvert.DeserializeObject<dynamic>(prova);

            string getFrom = test.GetFrom;

            string uriString;
            int height;
            int width;

            switch (getFrom)
            {
                case "Gallery":
                    uriString = test.Uri;
                    uri = Android.Net.Uri.Parse(uriString);
                    path = test.Path;
                    imgPhoto.SetImageURI(uri);
                    break;
                case "Camera":
                    uriString = test.Uri;
                    uri = Android.Net.Uri.Parse(uriString);
                    path = test.Path;

                    height = Resources.DisplayMetrics.HeightPixels;
                    width = 150;//imgPhoto.Height;
                    App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);

                    if (App.bitmap != null)
                    {
                        imgPhoto.SetImageBitmap(App.bitmap);
                        App.bitmap = null;
                    }

                    GC.Collect();
                    break;
                case "Received":
                    btnSendFile.Visibility = ViewStates.Gone;

                    uriString = test.Uri;
                    uri = Android.Net.Uri.Parse(uriString);
                    path = test.Path;

                    height = Resources.DisplayMetrics.HeightPixels;
                    width = 150;// imgPhoto.Height;
                    Bitmap bitmap = path.LoadAndResizeBitmap(width, height);

                    if (bitmap != null)
                    {
                        imgPhoto.SetImageBitmap(bitmap);
                        bitmap = null;
                    }

                    GC.Collect();
                    break;
                default:
                    break;
            }

            btnSendFile.Click += BtnSendFile_Click;
        }

        private void BtnSendFile_Click(object sender, EventArgs e)
        {
            //string packet = PacketManager.PackFile(PacketManager.readBytes(uri, 1), 1, uri.ToString());
            //socket.Send(packet);
            OnFirstSendFile(EventArgs.Empty, uri.ToString(), path);
            Intent chat = new Intent(this, typeof(ChatActivity));
            this.StartActivity(chat);
        }

        public override void OnBackPressed()
        {
            Intent chat = new Intent(this, typeof(ChatActivity));
            this.StartActivity(chat);
        }

        protected virtual void OnFirstSendFile(EventArgs args, string uri, string path)
        {
            firstSendFile?.Invoke(this, args, uri, path);
        }
    }

    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
}
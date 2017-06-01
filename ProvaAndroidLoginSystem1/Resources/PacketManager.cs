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
using ProvaAndroidLoginSystem1;
using ProvaAndroidLoginSystem1.Resources;
using Java.IO;

namespace p2p_project.Resources
{
    public delegate void MessageEventHandler(object sender, EventArgs e, string message);
    public delegate void FileEventHandler(object sender, EventArgs e, string uri);
    public delegate void UsernameEventHandler(object sender, EventArgs e);

    class PacketManager
    {
        public static event MessageEventHandler messageReceived;
        public static event UsernameEventHandler usernameReceived;
        public static event FileEventHandler fileReceived;
        private int number;
        private File f;
        private ISocket socket;

        public PacketManager(ISocket socket)
        {
            this.socket = socket;
        }

        protected virtual void OnMessageReceived(EventArgs e, string message)
        {
            messageReceived?.Invoke(this, e, message);
        }

        protected virtual void OnUsernameReceived(EventArgs e)
        {
            usernameReceived?.Invoke(this, e);
        }

        protected virtual void OnFileReceived(EventArgs e , string uri)
        {
            fileReceived?.Invoke(this, e, uri);
        }

        public object Unpack(string packet)
        {
            dynamic a = JsonConvert.DeserializeObject<dynamic>(packet);
            string type = a.Type;
            switch (type)
            {
                case "Username":
                    if(a.Buffer != "")
                    {
                        string Username = a.Buffer;
                        MainActivity.saveLocal("ConnectedUsername", Username);
                    }
                    OnUsernameReceived(EventArgs.Empty);
                    break;
                case "Message":
                    string message = a.Buffer;
                    OnMessageReceived(EventArgs.Empty, message);
                    break;
                case "File":
                    //scrivere il buffer
                    number = a.Number;
                    if(number == 1)
                    {
                        CreateFile();
                    }
                    Android.Net.Uri uri = a.Uri;
                    socket.Send(PackFile(readBytes(uri, number), number, uri.ToString()));
                    break;
                default:
                    //Errore
                    break;
            }
            return null;
        }

        private void CreateFile()
        {
            var fileDir = CreateDirectoryForPictures();
            f = new File(fileDir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
        }

        private void appendToFile(byte[] buffer)
        {
            
        }

        private Android.Net.Uri saveFile()
        {
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            Application.Context.SendBroadcast(mediaScanIntent);
            return contentUri;
        }

        private File CreateDirectoryForPictures()
        {
            File dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "ChatP2p");
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
            return dir;
        }

        public static string PackUsername(string username)
        {
            return JsonConvert.SerializeObject(new
                {
                    Type = "Username",
                    Buffer = MainActivity.retrieveLocal("Username") ?? "",
                    Checksum = ""
                });
        }

        public static string PackMessage(string message)
        {
            return JsonConvert.SerializeObject(new
                {
                    Type = "Message",
                    Buffer = message,
                    Checksum = ""
                });
        }

        public static string PackFile(byte[] buffer, int num, string uri)
        {
            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Number = num,
                Uri = uri,
                Buffer = buffer,
                Checksum = ""
            });
        }

        public byte[] readBytes(Android.Net.Uri uri, int number)
        {
            ContentResolver res = Application.Context.ContentResolver;
            // this dynamically extends to take the bytes you read
            var stream = res.OpenInputStream(uri);

            // this is storage overwritten on each iteration with bytes
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            // we need to know how may bytes were read to write them to the byteBuffer
            int len = 0;
            int i = 1;
            while ((len = stream.Read(buffer, 0, bufferSize)) != -1) {
                if (i == number)
                {
                    return buffer;
                }
                i++;
            }
            if(i > number)
            {
                OnFileReceived(EventArgs.Empty, uri.ToString());
            }
            return null;
        }
    }
}
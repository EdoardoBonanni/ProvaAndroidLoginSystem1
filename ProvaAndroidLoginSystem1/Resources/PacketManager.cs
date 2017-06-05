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
using System.IO;
using Android.Graphics;
using Android.Media;

namespace p2p_project.Resources
{
    public delegate void MessageEventHandler(object sender, EventArgs e, string message);
    public delegate void FileEventHandler(object sender, EventArgs e, string uri, bool mine);
    public delegate void UsernameEventHandler(object sender, EventArgs e);

    class PacketManager
    {
        public static event MessageEventHandler messageReceived;
        public static event UsernameEventHandler usernameReceived;
        public static event FileEventHandler fileReceived;

        private Dictionary<string, string> f;
        private ISocket socket;

        public PacketManager(ISocket socket)
        {
            this.socket = socket;
            this.f = new Dictionary<string, string>();
            SendFileActivity.firstSendFile += SendFileActivity_firstSendFile;
            new EventConsumer();
        }

        private void SendFileActivity_firstSendFile(object sender, EventArgs e, string uri)
        {
            string packet = PackFile(readBytes(Android.Net.Uri.Parse(uri), 1), 1, uri);
            socket.Send(packet);
        }

        protected virtual void OnMessageReceived(EventArgs e, string message)
        {
            messageReceived?.Invoke(this, e, message);
        }

        protected virtual void OnUsernameReceived(EventArgs e)
        {
            usernameReceived?.Invoke(this, e);
        }

        protected virtual void OnFileReceived(EventArgs e , string uri, bool mine)
        {
            fileReceived?.Invoke(this, e, uri, mine);
        }

        public void Unpack(string packet)
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
                    string command = a.Command;
                    switch (command)
                    {
                        case "Send":
                            int number = a.Number;
                            string path = "";
                            if (number == 1)
                            {
                                string uri = a.Path;
                                path = CreateFile(uri);
                            }
                            else
                            {
                                path = a.Path;
                            }
                            byte[] buffer = a.Buffer;
                            int num = appendToFile(path, buffer);
                            string pack = PackAck(num, path);
                            socket.Send(pack);
                            break;
                        case "Ack":
                            int numberAck = a.Number;
                            string pathAck = a.Path;
                            string uriAck = a.Buffer;

                            byte[] bufferAck = readBytes(Android.Net.Uri.Parse(uriAck), numberAck);
                            if (bufferAck != null)
                            {
                                if (bufferAck.Length == 0)
                                {
                                    socket.Send(PackEnd(pathAck));
                                }
                                else
                                {
                                    string packetAck = PackFile(bufferAck, numberAck, pathAck);
                                    socket.Send(packetAck);
                                }
                            }
                            else
                            {
                                //Nack
                            }
                            break;
                        case "Nack":

                            break;
                        case "End":
                            string pathEnd = a.Path;
                            saveFile(pathEnd);
                            OnFileReceived(EventArgs.Empty, f[pathEnd], false);
                            break;
                        default:
                            //Errore
                            break;
                    }
                    break;
                default:
                    //Errore
                    break;
            }
        }

        private string CreateFile(string uri)
        {
            var fileDir = CreateDirectoryForPictures();
            string fileName = String.Format("myPhoto_{0}.jpg", Guid.NewGuid());
            string path = System.IO.Path.Combine(fileDir.Path, fileName);
            f.Add(path, uri);
            FileStream fs = File.Create(path);
            fs.Close();
            return path;
        }

        private int appendToFile(string path, byte[] buffer)
        {
            FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write);

            fileStream.Write(buffer, 0, buffer.Length);

            var len = fileStream.Length;

            fileStream.Close();

            return getNumber(path);
        }

        private int getNumber(string path)
        {
            // this dynamically extends to take the bytes you read

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            int bufferSize = 16384;
            byte[] buffer = new byte[bufferSize];

            int len = 1;
            int i = 1;
            while ((len = fs.Read(buffer, 0, bufferSize)) > 0)
            {
                i++;
            }

            fs.Close();

            return i;
        }

        private void saveFile(string path)
        {
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.Parse(this.f[path]);
            mediaScanIntent.SetData(contentUri);
            Application.Context.SendBroadcast(mediaScanIntent);
        }

        private Java.IO.File CreateDirectoryForPictures()
        {
            Java.IO.File dir = new Java.IO.File(
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

        public static string PackFile(byte[] buffer, int num, string uri_Path)
        {
            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "Send",
                Number = num,
                Path = uri_Path,
                Buffer = buffer,
                Checksum = ""
            });
        }

        public string PackAck(int number, string path)
        {
            string uri = this.f[path];
            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "Ack",
                Number = number,
                Path = path,
                Buffer = uri,
                Checksum = ""
            });
        }

        public string PackEnd(string path)
        {
            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "End",
                Number = 2,
                Path = path,
                Checksum = ""
            });
        }

        public byte[] readBytes(Android.Net.Uri uri, int number)
        {
            ContentResolver res = Application.Context.ContentResolver;
            // this dynamically extends to take the bytes you read
            var stream = res.OpenInputStream(uri);

            // this is storage overwritten on each iteration with bytes
            int bufferSize = 16384;
            byte[] buffer = new byte[bufferSize];

            // we need to know how may bytes were read to write them to the byteBuffer
            int len = 0;
            int i = 0;
            while ((len = stream.Read(buffer, 0, bufferSize)) > 0) {
                i++;
                if (i == number)
                {
                    byte[] copyBuffer = new byte[len];
                    Array.Copy(buffer, copyBuffer, len);
                    return copyBuffer;
                }
                buffer = new byte[bufferSize];
            }
            if (number >= i)
            {
                OnFileReceived(EventArgs.Empty, uri.ToString(), true);
                return new byte[0];
            }
            return null;
        }
    }
}
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
    public delegate void FileEventHandler(object sender, EventArgs e, string fileName, string path, bool mine);
    public delegate void UsernameEventHandler(object sender, EventArgs e);

    class PacketManager
    {
        public static event MessageEventHandler messageReceived;
        public static event UsernameEventHandler usernameReceived;
        public static event FileEventHandler fileReceived;

        //<MittentePath, Tuple<Uri, DestinatarioPath>
        private Dictionary<string, Tuple<string, string>> mittente;
        //<DestinatarioPath, MittentePath>
        private Dictionary<string, string> destinatario;

        private ISocket socket;

        public PacketManager(ISocket socket)
        {
            this.socket = socket;
            this.mittente = new Dictionary<string, Tuple<string, string>>();
            this.destinatario = new Dictionary<string, string>();
            SendFileActivity.firstSendFile += SendFileActivity_firstSendFile;
            new EventConsumer();
        }

        private void SendFileActivity_firstSendFile(object sender, EventArgs e, string uri, string path)
        {
            this.mittente.Add(path, new Tuple<string, string>(uri, ""));
            string packet = PackFile(readBytes(Android.Net.Uri.Parse(uri), 1), 1, path);
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

        protected virtual void OnFileReceived(EventArgs e , string fileName, string path, bool mine)
        {
            fileReceived?.Invoke(this, e, fileName, path, mine);
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
                                string pathMittente = a.Path;
                                path = CreateFile();
                                destinatario.Add(path, pathMittente);
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
                            string pathDestinatario = a.Buffer;

                            addDestinatario(pathAck, pathDestinatario);

                            byte[] bufferAck = readBytes(Android.Net.Uri.Parse(mittente[pathAck].Item1), numberAck);
                            if (bufferAck != null)
                            {
                                if (bufferAck.Length == 0)
                                {
                                    OnFileReceived(EventArgs.Empty, System.IO.Path.GetFileName(pathAck), pathAck, true);
                                    socket.Send(PackEnd(pathDestinatario));
                                    mittente.Remove(pathAck);
                                }
                                else
                                {
                                    string packetAck = PackFile(bufferAck, numberAck, pathDestinatario);
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
                            string pathEndDestinatario = a.Path;
                            OnFileReceived(EventArgs.Empty, System.IO.Path.GetFileName(pathEndDestinatario), pathEndDestinatario, false);
                            saveFile(pathEndDestinatario);
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

        private void addDestinatario(string pathMittente, string pathDestinatario)
        {
            if (mittente[pathMittente].Item2.Equals(""))
            {
                string uri = mittente[pathMittente].Item1;
                mittente[pathMittente] = new Tuple<string, string>(uri, pathDestinatario);
            }
        }

        private string CreateFile()
        {
            var fileDir = CreateDirectoryForPictures("ChatP2p Ricevute");
            string fileName = String.Format("myPhoto_{0}.jpg", Guid.NewGuid());
            string path = System.IO.Path.Combine(fileDir.Path, fileName);
            FileStream fs = File.Create(path);
            fs.Close();
            return path;
        }

        private Java.IO.File CreateDirectoryForPictures(string dirName)
        {
            string path = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures).Path, "ChatP2p");
            Java.IO.File dir = new Java.IO.File(path, dirName);
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
            return dir;
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
            Java.IO.File file = new Java.IO.File(path);

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(file);
            mediaScanIntent.SetData(contentUri);
            Application.Context.SendBroadcast(mediaScanIntent);

            destinatario.Remove(path);
        }

        #region pack
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

        public static string PackFile(byte[] buffer, int num, string path)
        {
            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "Send",
                Number = num,
                Path = path,
                Buffer = buffer,
                Checksum = ""
            });
        }

        public string PackAck(int number, string pathDestinatario)
        {
            string pathMittente = destinatario[pathDestinatario];

            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "Ack",
                Number = number,
                Path = pathMittente,
                Buffer = pathDestinatario,
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
        #endregion pack

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
                return new byte[0];
            }
            return null;
        }
    }
}
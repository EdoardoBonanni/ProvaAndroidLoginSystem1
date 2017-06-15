using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Newtonsoft.Json;
using ProvaAndroidLoginSystem1;
using System.IO;

namespace p2p_project.Resources
{
    public delegate void MessageEventHandler(object sender, EventArgs e, string message);
    public delegate void FileEventHandler(object sender, EventArgs e, string fileName, string path, bool mine);
    public delegate void UsernameEventHandler(object sender, EventArgs e);
    public delegate void PartFileEventHandler(object sender, EventArgs e, string fileName, string path, bool mine, int number, int totalNumber);

    class PacketManager
    {
        public static event MessageEventHandler messageReceived;
        public static event UsernameEventHandler usernameReceived;
        public static event FileEventHandler fileReceived;
        public static event PartFileEventHandler partFileReceived;

        //<MittentePath, Tuple<Uri, DestinatarioPath>
        private Dictionary<string, Tuple<string, string>> mittente;
        //<DestinatarioPath, MittentePath>
        private Dictionary<string, Tuple<string, int>> destinatario;

        private ISocket socket;

        public PacketManager(ISocket socket)
        {
            this.socket = socket;
            this.mittente = new Dictionary<string, Tuple<string, string>>();
            this.destinatario = new Dictionary<string, Tuple<string, int>>();
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

        protected virtual void OnPartFileReceived(EventArgs e, string fileName, string path, bool mine, int number, int totalNumber)
        {
            partFileReceived?.Invoke(this, e, fileName, path, mine, number, totalNumber);
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
                                int totalNumber1 = a.TotalNumber;
                                string pathMittente = a.Path;
                                path = CreateFile();
                                destinatario.Add(path, new Tuple<string, int>(pathMittente, totalNumber1));
                            }
                            else
                            {
                                path = a.Path;
                            }
                            byte[] buffer = a.Buffer;
                            var totalNumber = getTotalNumber(path, false);
                            OnPartFileReceived(EventArgs.Empty, System.IO.Path.GetFileName(path), path, false, number, totalNumber);
                            int num = appendToFile(path, buffer);
                            string pack = PackAck(num, path);
                            socket.Send(pack);
                            break;
                        case "Ack":
                            int numberAck = a.Number;
                            string pathAck = a.Path;
                            string pathDestinatario = a.Buffer;
                            var totalNumberAck = this.getTotalNumber(pathAck, true);
                            OnPartFileReceived(EventArgs.Empty, System.IO.Path.GetFileName(pathAck), pathAck, true, numberAck, totalNumberAck);
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
                            string newPath = saveFile(pathEndDestinatario);
                            OnFileReceived(EventArgs.Empty, System.IO.Path.GetFileName(newPath), newPath, false);
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
            var tempDir = Application.Context.ExternalCacheDir;
            string dirName = "ChatP2p Temporanee";

            Java.IO.File dir = new Java.IO.File(tempDir.Path, dirName);
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }

            string fileName = string.Format("myPhoto_{0}.jpg", Guid.NewGuid());
            string path = System.IO.Path.Combine(dir.Path, fileName);

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

            int bufferSize = 4096;
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

        private string saveFile(string path)
        {
            Java.IO.File file = new Java.IO.File(path);

            var dir = CreateDirectoryForPictures("ChatP2p ricevute");
            string newPath = System.IO.Path.Combine(dir.Path, System.IO.Path.GetFileName(path));

            Java.IO.File newFile = new Java.IO.File(newPath);

            var tempBites = File.ReadAllBytes(path);
            File.WriteAllBytes(newPath, tempBites);

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(newFile);
            mediaScanIntent.SetData(contentUri);
            Application.Context.SendBroadcast(mediaScanIntent);

            destinatario.Remove(path);
            File.Delete(path);

            return newPath;
        }

        #region pack
        public static string PackUsername(string username)
        {
            return JsonConvert.SerializeObject(new
                {
                    Type = "Username",
                    Buffer = MainActivity.retrieveLocal("Username") ?? ""
                });
        }

        public static string PackMessage(string message)
        {
            return JsonConvert.SerializeObject(new
                {
                    Type = "Message",
                    Buffer = message
                });
        }

        public static string PackFile(byte[] buffer, int num, string path)
        {
            if (num == 1)
            {
                int totalNumber = getTotalNumber(path);
                return JsonConvert.SerializeObject(new
                {
                    Type = "File",
                    Command = "Send",
                    Number = num,
                    TotalNumber = totalNumber,
                    Path = path,
                    Buffer = buffer
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    Type = "File",
                    Command = "Send",
                    Number = num,
                    Path = path,
                    Buffer = buffer
                });
            }
        }

        public string PackAck(int number, string pathDestinatario)
        {
            string pathMittente = destinatario[pathDestinatario].Item1;

            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "Ack",
                Number = number,
                Path = pathMittente,
                Buffer = pathDestinatario
            });
        }

        public string PackEnd(string path)
        {
            return JsonConvert.SerializeObject(new
            {
                Type = "File",
                Command = "End",
                Number = 2,
                Path = path
            });
        }
        #endregion pack

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

        private static int getTotalNumber(string path)
        {
            Java.IO.File f = new Java.IO.File(path);
            var fileLength = f.Length();
            var number = fileLength / 4096;
            if (fileLength % 4096 > 0)
            {
                number++;
            }
            return Convert.ToInt32(number);
        }

        public int getTotalNumber(string path, bool mine)
        {
            if (mine)
            {
                return getTotalNumber(path);
            }
            else
            {
                return destinatario[path].Item2;
            }
        }
    }
}
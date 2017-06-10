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
using p2p_project.Resources.Model;

namespace p2p_project
{
    class ChatAdapter : BaseAdapter
    {

        List<Tuple<Registro, bool>> chat = new List<Tuple<Registro, bool>>();
        Activity context;

        public ChatAdapter(Activity context, List<Tuple<Registro, bool>> chat)
        {
            this.chat = chat.OrderBy(d => d.Item1.Orario).ToList();
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return this.chat[position].Item1;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Chat_template_layout, parent, false);

            var Chat = view.FindViewById<TextView>(Resource.Id.txtAdapter);
            var Orario = view.FindViewById<TextView>(Resource.Id.txtOrario);

            //var date = chat.GroupBy(o => o.Item1.Orario.Date).Select(a => a.First()).ToList();

            if (chat[position] != null)
            {
                Chat.Text = chat[position].Item1.Messaggio;
                Orario.Text = chat[position].Item1.Orario.ToString("dd/MM/yyy hh:mm");

                if(chat[position].Item2)
                {
                    Chat.Gravity = GravityFlags.Right;
                    Orario.Gravity = GravityFlags.Right;
                }
                else
                {
                    Chat.Gravity = GravityFlags.Left;
                    Orario.Gravity = GravityFlags.Left;
                }
            }

            return view;
        }

        public void update(string message, bool mine, string path, bool isFile)
        {
            if (isFile)
            {
                var receivingFile = chat.Where(a => a.Item1.Path == path).FirstOrDefault();

                if (receivingFile != null)
                {
                    chat.Remove(receivingFile);
                }
            }

            chat.Add(new Tuple<Registro, bool>(new Registro {
                Messaggio = message,
                Orario = DateTime.Now,
                Path = path,
                isFile = isFile
            }, mine));

            chat = chat.OrderBy(d => d.Item1.Orario).ToList();
            NotifyDataSetChanged();
        }
        
        public bool test(string path)
        {
            var receivingFile = chat.Where(a => a.Item1.Path == path).FirstOrDefault();
            if(receivingFile != null)
            {
                return false;
            }
            return true;
        }

        public override int Count
        {
            get
            {
                return this.chat.Count;
            }
        }

    }
}
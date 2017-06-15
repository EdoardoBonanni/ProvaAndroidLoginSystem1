using System;

namespace p2p_project.Resources
{
    interface ISocket
    {
        int Connect();

        void Send(string packet);

        void Receive();

        void receiveCallback(IAsyncResult res);

        void End();
    }
}
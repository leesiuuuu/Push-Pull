using System;

namespace Mirror.FizzySteam
{
    public interface IServer
    {
        void ReceiveData();
        void Send(int connectionId, ArraySegment<byte> segment, int channelId);
        void Disconnect(int connectionId);
        void FlushData();
        string ServerGetClientAddress(int connectionId);
        void Shutdown();
    }
}
using System;

namespace Mirror.FizzySteam
{
    public interface IClient
    {
        bool Connected { get; }
        bool Error { get; }


        void ReceiveData();
        void Disconnect();
        void FlushData();
        void Send(ArraySegment<byte> segment, int channelId);
    }
}
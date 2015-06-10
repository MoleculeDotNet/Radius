using System;
using Microsoft.SPOT;

namespace IngenuityMicro.Radius
{
    public delegate void PeerChannelReceivedHandler(string val);

    public interface IPeerChannel
    {
        event PeerChannelReceivedHandler DataReceived;

        void Send(string text);
    }
}

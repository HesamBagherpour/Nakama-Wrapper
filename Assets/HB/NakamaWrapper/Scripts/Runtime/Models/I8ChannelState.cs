using System;
using Nakama;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8ChannelState
    {
        public  event Action<IChannelPresenceEvent> I8OnChannelPresenceEvent;

        public void Raise(IChannelPresenceEvent channelMessage);
        
    }
}
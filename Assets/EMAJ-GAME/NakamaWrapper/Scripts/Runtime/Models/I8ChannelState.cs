using System;
using Nakama;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8ChannelState
    {
        public  event Action<IChannelPresenceEvent> I8OnChannelPresenceEvent;

        public void Raise(IChannelPresenceEvent channelMessage);
        
    }
}
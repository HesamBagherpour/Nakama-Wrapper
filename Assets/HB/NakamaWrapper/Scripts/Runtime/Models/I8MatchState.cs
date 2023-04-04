using System;
using Nakama;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8MatchState
    {
        public  event Action<IMatchState> I8OnReceivedChannelMessage;

        public void Raise(IMatchState channelMessage);


    }
}
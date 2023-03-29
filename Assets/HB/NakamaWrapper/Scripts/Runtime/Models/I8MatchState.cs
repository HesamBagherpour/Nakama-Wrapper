using System;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Models
{
    public interface I8MatchState
    {
        public  event Action<IMatchState> I8OnReceivedChannelMessage;

        public void Raise(IMatchState channelMessage);


    }
}
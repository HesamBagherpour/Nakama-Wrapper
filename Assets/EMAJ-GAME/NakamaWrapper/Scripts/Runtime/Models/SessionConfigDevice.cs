﻿namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Models
{
    public class SessionConfigDevice: SessionConfig
    {
        public  string UniqueIdentifier;
        public SessionConfigDevice(string uniqueIdentifier)
        {
            this.UniqueIdentifier = uniqueIdentifier;
        }
    }
}
﻿using System;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Session;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Factory;
using Nakama;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Core
{
    public  class EM_Session
    {
        public string tag;
        public ISession Session;
        public SocketFactory SocketFactory;
        public SessionConfig sessionConfig;
        public EM_Client client;
        public SessionConnectionController sessionConnectionController;
        
        public async UniTask<Tuple<bool, EM_Session>> CreateSession<T>(string tag,EM_Client client ,T sessionConfig) where T : SessionConfig
        {
            SocketFactory = new SocketFactory();
            switch (typeof(T))
            {
                case
                    var cl when cl== typeof(SessionConfigDevice):{
                    SessionConfigDevice s = sessionConfig as SessionConfigDevice;
                    Session = await client.client.AuthenticateDeviceAsync(s.UniqueIdentifier);
                    this.tag = tag;
                    break;
                }
                case
                    var cl when cl == typeof(SessionConfigEmail):{
                    SessionConfigEmail s = sessionConfig as SessionConfigEmail;
                    Session = await client.client.AuthenticateEmailAsync(s.username,s.password);
                    this.tag = tag;
                    break;
                }
            }
            return new Tuple<bool, EM_Session>(true, this);
        }

        public void setSessionConnectionController(SessionConnectionController sessionConnectionController)
        {
            this.sessionConnectionController = sessionConnectionController;
        }




    }
    
    

}
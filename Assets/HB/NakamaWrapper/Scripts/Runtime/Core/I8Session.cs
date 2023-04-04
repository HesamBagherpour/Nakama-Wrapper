using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Controllers.Session;
using HB.NakamaWrapper.Scripts.Runtime.Factory;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Core
{
    public  class I8Session
    {
        public string tag;
        public ISession Session;
        public SocketFactory SocketFactory;
        public SessionConfig sessionConfig;
        public I8Client client;

        public SessionConnectionController sessionConnectionController;
        
        public async UniTask<Tuple<bool, I8Session>> CreateSession<T>(string tag,I8Client client ,T sessionConfig) where T : SessionConfig
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
            return new Tuple<bool, I8Session>(true, this);
        }

        public void setSessionConnectionController(SessionConnectionController sessionConnectionController)
        {
            this.sessionConnectionController = sessionConnectionController;
        }




    }
    
    

}
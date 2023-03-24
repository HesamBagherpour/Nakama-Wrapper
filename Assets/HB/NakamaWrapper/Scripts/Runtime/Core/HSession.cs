using System;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Controller;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;

namespace HB.NakamaWrapper.Scripts.Runtime.Core
{
    public  class HSession
    {
        public string tag;
        public ISession Session;
        public SocketFactory SocketFactory;
        public SessionConfig sessionConfig;
        public HClient client;

        
        public async UniTask<Tuple<bool, HSession>> createSession<T>(string tag,HClient client ,T sessionConfig) where T : SessionConfig
        {
            SocketFactory = new SocketFactory();
            switch (typeof(T))
            {
                case
                    var cl when cl== typeof(SessionConfigDevice):{
                    SessionConfigDevice s = sessionConfig as SessionConfigDevice;
                    Session = await client.client.AuthenticateDeviceAsync(s.UniqueIdentifier);
                    break;
                }
                case
                    var cl when cl == typeof(SessionConfigEmail):{
                    SessionConfigEmail s = sessionConfig as SessionConfigEmail;
                    Session = await client.client.AuthenticateEmailAsync(s.username,s.password);
                    
                    break;
                }
            }
            
            return new Tuple<bool, HSession>(true, this);

        }




    }
    
    

}
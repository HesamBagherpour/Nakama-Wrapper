using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controller
{
    public class SessionFactory
    {
        private List<I8Session> _sessions;
        public async UniTask<Tuple<bool, I8Session>> CreateSession<T>(string tag,I8Client client ,T config) where T : SessionConfig
        {
            //TODO check if not exist tag or name - return error if exist
            _sessions = new List<I8Session>();
            var session = new I8Session();
            // session = _sessions.Find(x => x.tag == tag);
            // if (session != null)
            //     return new Tuple<bool, I8Session>(false,null);
            
            var  (_ ,msession ) = await session.createSession(tag,client,config);
            return new Tuple<bool, I8Session>(true, msession);
        }
        public async UniTask<Tuple<bool, I8Session>> CreateOrGetSession<T>(string tag,I8Client client ,T config) where T : SessionConfig
        {
            
            var session = new I8Session();
            session = _sessions.Find(x => x.tag == tag);
            if (session != null)
                return new Tuple<bool, I8Session>(true,session);
            var  (_ ,msession ) = await session.createSession(tag,client,config);
            _sessions.Add(msession);
            return new Tuple<bool, I8Session>(true, msession);
            
        }
        public I8Session GetSession(string tag)
        {
            return _sessions.Find(x => x.tag == tag);
        }
    }
}
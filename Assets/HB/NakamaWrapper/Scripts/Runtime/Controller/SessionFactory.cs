using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using UnityEngine;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Controller
{
    public class SessionFactory
    {
        private List<HSession> _sessions;
        public async UniTask<Tuple<bool, HSession>> CreateSession<T>(string tag,HClient client ,T config) where T : SessionConfig
        {
            //TODO check if not exist tag or name - return error if exist
            _sessions = new List<HSession>();
            var session = new HSession();
            // session = _sessions.Find(x => x.tag == tag);
            // if (session != null)
            //     return new Tuple<bool, I8Session>(false,null);
            
            var  (_ ,msession ) = await session.createSession(tag,client,config);
            return new Tuple<bool, HSession>(true, msession);
        }
        public async UniTask<Tuple<bool, HSession>> CreateOrGetSession<T>(string tag,HClient client ,T config) where T : SessionConfig
        {
            
            var session = new HSession();
            session = _sessions.Find(x => x.tag == tag);
            if (session != null)
                return new Tuple<bool, HSession>(true,session);
            var  (_ ,msession ) = await session.createSession(tag,client,config);
            _sessions.Add(msession);
            return new Tuple<bool, HSession>(true, msession);
            
        }
        public HSession GetSession(string tag)
        {
            return _sessions.Find(x => x.tag == tag);
        }
    }
}
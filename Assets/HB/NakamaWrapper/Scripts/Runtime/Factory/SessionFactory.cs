using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;

namespace HB.NakamaWrapper.Scripts.Runtime.Factory
{
    public class SessionFactory
    {
        private List<I8Session> _8sessions = new List<I8Session>();
        public Action<I8Session> OnCreateSession;
        public string latestTagCreated;
        public async UniTask<Tuple<bool, I8Session>> CreateSession<T>(string tag,I8Client client ,T config) where T : SessionConfig
        {
            var session = _8sessions.Find(x => x.tag == tag);
            if (session != null)
                return new Tuple<bool,I8Session>(true,session);

            var _session = new I8Session();
            var  (_ ,msession ) = await _session.CreateSession(tag,client,config);
            _8sessions.Add(msession);
            OnCreateSession?.Invoke(msession);
            latestTagCreated = tag;
            return new Tuple<bool, I8Session>(true, msession);
        }
        public async UniTask<Tuple<bool, I8Session>> CreateOrGetSession<T>(string tag,I8Client client ,T config) where T : SessionConfig
        {
            
            //TODO check if not exist tag or name - return error if exist
            var _sesssion = _8sessions.Find(x => x.tag == tag);
            if (_sesssion != null)
                return new Tuple<bool, I8Session>(true,_sesssion);
            return await CreateSession(tag,client, config);
            
        }
        
        
        public async UniTask<I8Session> GetSession(string tag)
        {

            if (_8sessions.Exists(x => x.tag == tag))
            {
                return _8sessions.Find(x => x.tag == tag);
            }
            else
            {
                await UniTask.WaitUntil(() => latestTagCreated == tag );
                return _8sessions.Find(x => x.tag == tag);
            }
        }
    }
}
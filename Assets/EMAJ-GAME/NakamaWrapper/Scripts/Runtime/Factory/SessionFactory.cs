using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory
{
    public class SessionFactory
    {
        private List<EM_Session> _8sessions = new List<EM_Session>();
        public Action<EM_Session> OnCreateSession;
        public string latestTagCreated;
        public async UniTask<Tuple<bool, EM_Session>> CreateSession<T>(string tag,EM_Client client ,T config) where T : SessionConfig
        {
            var session = _8sessions.Find(x => x.tag == tag);
            if (session != null)
                return new Tuple<bool,EM_Session>(true,session);

            var _session = new EM_Session();
            var  (_ ,msession ) = await _session.CreateSession(tag,client,config);
            _8sessions.Add(msession);
            OnCreateSession?.Invoke(msession);
            latestTagCreated = tag;
            return new Tuple<bool, EM_Session>(true, msession);
        }
        public async UniTask<Tuple<bool, EM_Session>> CreateOrGetSession<T>(string tag,EM_Client client ,T config) where T : SessionConfig
        {
            
            //TODO check if not exist tag or name - return error if exist
            var _sesssion = _8sessions.Find(x => x.tag == tag);
            if (_sesssion != null)
                return new Tuple<bool, EM_Session>(true,_sesssion);
            return await CreateSession(tag,client, config);
            
        }
        
        
        public async UniTask<EM_Session> GetSession(string tag)
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
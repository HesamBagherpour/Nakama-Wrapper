using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;
using Nakama.TinyJson;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Components
{
    public sealed class OpCodeGenerator : MonoBehaviour
    {
        

        public ISession Session;
        public IClient Client;
        public ISocket Socket;
        public string matchId;
        public string matchTagName;
        public string sessionTagName;
        public List<OpCodeCompModel> opCodes = new List<OpCodeCompModel>();
        public bool isInitNetworkEvents;
        public string userName;
        public Dictionary<string, string> opCodeUuidServerConfig;
        public Action<long, string, string, IMatchState> OnReceiveOpCodeMessage;
        public MatchConnectionController _matchConnectionController;
        public MatchOpCodeController _matchOpCodeController;

        public Action Onconnected;

        
        #region Init
        public void Init(MatchConnectionController matchConnectionController , MatchOpCodeController matchOpCodeController)
        {
            _matchConnectionController = matchConnectionController;
            _matchOpCodeController = matchOpCodeController;
            Debug.unityLogger.Log("MultiPlayerNetworkSync | Init | start");
            isInitNetworkEvents = false;
            if (_matchConnectionController.isConnected)
            {
                Debug.unityLogger.Log("MultiPlayerNetworkSync | Init | isConnected call onConnect");
                ONConnect();
            }
            else
            {
                Debug.unityLogger.Log("MultiPlayerNetworkSync | Init | not connected register OnConnect");
                //TODO
                _matchConnectionController.OnMatchConnect += OnMatchConnect;
            }
        }
        private void Init(MatchConnectionController matchConnectionController , MatchOpCodeController matchOpCodeController,Dictionary<string, string> opCodeUuidServerConfig)
        {
            this.opCodeUuidServerConfig = opCodeUuidServerConfig;
            Init(matchConnectionController ,matchOpCodeController);
        }
        private void Init(MatchConnectionController matchConnectionController , MatchOpCodeController matchOpCodeController,string userName)
        {
            this.userName = userName;
            Init(matchConnectionController ,matchOpCodeController);
        }

        private void OnMatchConnect(string matchId, string matchTag,EM_Socket socket)
        {
            this.matchId = matchId;
            matchTagName = matchTag;
            ONConnect();
        }
        
        #endregion
        #region sendMatchState
        public async UniTask SendMatchState(long opCode, string state, IEnumerable<IUserPresence> presences = null)
        {
            if (!isInitNetworkEvents)
                await UniTask.WaitUntil(() => isInitNetworkEvents);
            await _matchOpCodeController.matchMessageController.SendMatchState(opCode, state, presences = null);
        }
        public async UniTask SendMatchState(long opCode, ArraySegment<byte> state, IEnumerable<IUserPresence> presences = null)
        {
            if (!isInitNetworkEvents)
                await UniTask.WaitUntil(() => isInitNetworkEvents);
            await _matchOpCodeController.matchMessageController.SendMatchState(opCode, state, presences);
        }
        public async UniTask SendMatchState(long opCode, byte[] state, IEnumerable<IUserPresence> presences = null)
        {
            if (!isInitNetworkEvents)
                await UniTask.WaitUntil(() => isInitNetworkEvents);
            await _matchOpCodeController.matchMessageController.SendMatchState(opCode, state, presences);
        }
        #endregion
        #region ReeeiveMessage
        private void ONReceiveOpCodeMessage(long opCode, string key, IMatchState state) {
                var a = Encoding.UTF8.GetString(state.State).FromJson<MultiPlayerMessage<object>>();
                //var packet =JsonConvert.DeserializeObject<MultiPlayerMessage<PingPongMessage>>(Encoding.UTF8.GetString(state.State)) ;
                if (opCodes.Exists(model => model.Uuid == a.uuid))
                {
                    OnReceiveOpCodeMessage?.Invoke(opCode, key, a.uuid, state);
                }
        }
        #endregion
        #region Connect

            private async void ONConnect()
            {
                Debug.unityLogger.Log("MultiPlayerNetworkSync | onConnect | start");
                foreach (OpCodeCompModel opCode in opCodes)
                {
                    Debug.unityLogger.Log($"MultiPlayerNetworkSync | onConnect | await register {opCode.Key} ");
                    //InitOpCodeUuid(opCode);
                    opCode.OpCode =
                        await _matchOpCodeController.RegisterOpCode(opCode.Key,
                            ONReceiveOpCodeMessage , opCode.OpCode);
                    Debug.unityLogger.Log($"MultiPlayerNetworkSync | onConnect | after register {opCode.Key} {opCode.OpCode} ");
                }

                isInitNetworkEvents = true;
                Debug.unityLogger.Log($"MultiPlayerNetworkSync | onConnect | end isInitNetworkEvents {isInitNetworkEvents} ");
                Onconnected.Invoke();
            }


        #endregion
        #region OpCode

            private void InitOpCodeUuid(OpCodeCompModel opCodeComp)
            {
                if (opCodeComp.UuidGeneratorType == OpCodeUuidGeneratorType.ServerConfig)
                {
                    opCodeComp.Uuid = opCodeUuidServerConfig[opCodeComp.Key];
                }
                else if (opCodeComp.UuidGeneratorType == OpCodeUuidGeneratorType.MultiPlayerNetwork)
                {
                    opCodeComp.Uuid = userName;
                }
            }
            
            public OpCodeCompModel GetOpCode(long opCode = 0, string key = null, string uuid = null)
            {
                if (opCode != 0)
                    return opCodes.Find(model => model.OpCode == opCode);
                if (key != null)
                    return opCodes.Find(model => model.Key == key);
                if (uuid != null)
                    return opCodes.Find(model => model.Uuid == uuid);
                return null;
        }

        #endregion
        #region Editor
#if UNITY_EDITOR
        
        [ContextMenu("Create new static UUID")]
        public void UpdateUuid()
        {
            foreach (OpCodeCompModel opCode in opCodes)
            {
                string uuidTemp = "";
                if (opCode.UuidGeneratorType == OpCodeUuidGeneratorType.Static)
                {
                    opCode.Uuid = "";
                    uuidTemp += GetInstanceID() + opCode.Key + DateTime.Now.ToUniversalTime();
                    // opCode.Uuid = GeneralUtility.GenerateMD5(uuidTemp);
                    opCode.Uuid = Random.Range(1000,9999999).ToString();
                }
            }
            EditorUtility.SetDirty(this);
        }

#endif
        
        #endregion
        
    }
}
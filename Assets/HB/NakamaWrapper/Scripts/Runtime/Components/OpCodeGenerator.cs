using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Nakama.TinyJson;
using UnityEditor;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Components
{
    public class OpCodeGenerator : MonoBehaviour
    {
        
        //client Tag
        //session Tag
        //socket Tag
        //match Tag
        //match id

         public List<OpCodeCompModel> opCodes = new List<OpCodeCompModel>();
        public bool initOnStart;
        public bool isInitNetworkEvents;
        public string userName;
        // key: opCodeKey , value: uuid
        public Dictionary<string, string> opCodeUuidServerConfig;
        public Action<long, string, string, IMatchState> OnReceiveOpCodeMessage;
        // public Dictionary<long, string, IMatchState> OnReceiveOpCodeMessage;

        private MatchConnectionController _matchConnectionController;
        private MatchOpCodeController _matchOpCodeController;
        
        protected internal virtual void Start()
        {
            //TODO by tag
            // if (initOnStart)
                // Init();
        }

        public virtual void Init(MatchConnectionController matchConnectionController , MatchOpCodeController matchOpCodeController)
        {
            _matchConnectionController = matchConnectionController;
            _matchOpCodeController = matchOpCodeController;
            Debug.unityLogger.Log("MultiPlayerNetworkSync | Init | start");
            isInitNetworkEvents = false;
            if (_matchConnectionController.isConnected)
            {
                Debug.unityLogger.Log("MultiPlayerNetworkSync | Init | isConnected call onConnect");
                onConnect();
            }
            else
            {
                Debug.unityLogger.Log("MultiPlayerNetworkSync | Init | not connected register OnConnect");
                //TODO
                // MultiPlayerManager.Instance.OnConnect += onConnect;
            }
        }
        public virtual void Init(MatchConnectionController matchConnectionController , MatchOpCodeController matchOpCodeController,Dictionary<string, string> opCodeUuidServerConfig)
        {
            this.opCodeUuidServerConfig = opCodeUuidServerConfig;
            Init(matchConnectionController ,matchOpCodeController);
        }
        public virtual void Init(MatchConnectionController matchConnectionController , MatchOpCodeController matchOpCodeController,string userName)
        {
            this.userName = userName;
            Init(matchConnectionController ,matchOpCodeController);
        }


        public async UniTask SendGameState(long opCode, string state, IEnumerable<IUserPresence> presences = null)
        {
            if (!isInitNetworkEvents)
                await UniTask.WaitUntil(() => isInitNetworkEvents);
            await _matchOpCodeController._matchMessageController.SendMatchState(opCode, state, presences = null);
        }

        public async UniTask SendGameState(long opCode, ArraySegment<byte> state, IEnumerable<IUserPresence> presences = null)
        {
            if (!isInitNetworkEvents)
                await UniTask.WaitUntil(() => isInitNetworkEvents);
            await _matchOpCodeController._matchMessageController.SendMatchState(opCode, state, presences);
        }

        public async UniTask SendGameState(long opCode, byte[] state, IEnumerable<IUserPresence> presences = null)
        {
            if (!isInitNetworkEvents)
                await UniTask.WaitUntil(() => isInitNetworkEvents);
            await _matchOpCodeController._matchMessageController.SendMatchState(opCode, state, presences);
        }


        protected internal async void onConnect()
        {
            Debug.unityLogger.Log("MultiPlayerNetworkSync | onConnect | start");
            foreach (OpCodeCompModel opCode in opCodes)
            {
                Debug.unityLogger.Log($"MultiPlayerNetworkSync | onConnect | await register {opCode.Key} ");
                initOpCodeUuid(opCode);
                opCode.OpCode =
                    await _matchOpCodeController.RegisterOpCode(opCode.Key,
                        onReceiveOpCodeMessage);
                Debug.unityLogger.Log($"MultiPlayerNetworkSync | onConnect | after register {opCode.Key} {opCode.OpCode} ");
            }

            isInitNetworkEvents = true;
            Debug.unityLogger.Log($"MultiPlayerNetworkSync | onConnect | end isInitNetworkEvents {isInitNetworkEvents} ");
        }
        protected internal virtual void onReceiveOpCodeMessage(long opCode, string key, IMatchState state)
        {
            var a = Encoding.UTF8.GetString(state.State).FromJson<MultiPlayerMessage<object>>();
            if (opCodes.Exists(model => model.Uuid == a.uuid))
                OnReceiveOpCodeMessage?.Invoke(opCode, key, a.uuid, state);
        }

        #if UNITY_EDITOR

        [ContextMenu("Create new static UUID")]
        void UpdateUUID()
        {
            foreach (OpCodeCompModel opCode in opCodes)
            {
                string uuidTemp = "";
                if (opCode.UuidGeneratorType == OpCodeUuidGeneratorType.Static)
                {
                    opCode.Uuid = "";
                    uuidTemp += GetInstanceID() + opCode.Key + DateTime.Now.ToUniversalTime();
                    // opCode.Uuid = GeneralUtility.GenerateMD5(uuidTemp);
                    opCode.Uuid = UnityEngine.Random.Range(1000,9999999).ToString();
                }
            }
            EditorUtility.SetDirty(this);
        }

        #endif

        private void initOpCodeUuid(OpCodeCompModel opCodeComp)
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

        public OpCodeCompModel getOpCode(long opCode = 0, string key = null, string uuid = null)
        {
            if (opCode != 0)
                return opCodes.Find(model => model.OpCode == opCode);
            if (key != null)
                return opCodes.Find(model => model.Key == key);
            if (uuid != null)
                return opCodes.Find(model => model.Uuid == uuid);
            return null;
        }

    }
}
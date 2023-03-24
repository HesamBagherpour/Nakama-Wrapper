using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace HB.NakamaWrapper.Scripts.Runtime.Component
{
    public class OpCodeGenerator : MonoBehaviour
    {



        [SerializeField] private Dictionary<string, long> _opCodeByKey = new Dictionary<string, long>();
           public MatchConnectionController _matchConnectionController;
           public MatchMessageController _matchMessageController;
        [SerializeField]private List<OpCodeCompModel> opCodes;
        private Dictionary<string, string> _opCodeUUidServerConfig;
        private string _userName;
        private string _matchId;
        public Dictionary<long, UnityEvent<long, string, IMatchState>> opCodeCallbacks =
            new Dictionary<long, UnityEvent<long, string, IMatchState>>();

        

        #region Init
        private void Awake()
        {
            _opCodeByKey = new Dictionary<string, long>();
            _matchMessageController = GetComponent<MatchMessageController>();
            _matchConnectionController = GetComponent<MatchConnectionController>();
        }
        public void Init(string userName,string matchId)
        {
            Debug.Log("Init OpCode Here : ");
            _matchConnectionController.OnMatchConnect += OnMatchConnect;
            _userName = userName;
            _matchId = matchId;
        }
        
        private void InitOpCodeUuid(OpCodeCompModel opCodeComp)
        {
            if (opCodeComp.UuidGeneratorType == OpCodeUuidGeneratorType.ServerConfig)
            {
                opCodeComp.Uuid = _opCodeUUidServerConfig[opCodeComp.Key];
            }
            else if (opCodeComp.UuidGeneratorType == OpCodeUuidGeneratorType.MultiPlayerNetwork)
            {
                opCodeComp.Uuid = _userName;
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
                    InitOpCodeUuid(opCode);
                    opCode.OpCode =
                        await RegisterOpCode(opCode.Key,
                            _matchMessageController.ONReceiveOpCodeMessage);
                    Debug.unityLogger.Log(
                        $"MultiPlayerNetworkSync | onConnect | after register {opCode.Key} {opCode.OpCode} ");
                }
            }
            
            
            private void OnMatchConnect(string matchId)
            {
                ONConnect();
                _matchId = matchId;
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
                if (opCode.UuidGeneratorType != OpCodeUuidGeneratorType.Static) continue;
                opCode.Uuid = "";
                uuidTemp += GetInstanceID() + opCode.Key + DateTime.Now.ToUniversalTime();
            } 
            EditorUtility.SetDirty(this);
        }
#endif

        #endregion
        #region OpCode

        private async UniTask<long> RegisterOpCode(string key, UnityAction<long, string, IMatchState> callback)
        {
            Debug.Log("RegisterOpCode start");
            if (_opCodeByKey.ContainsKey(key))
            {
                Debug.Log($"RegisterOpCode ContainsKey(key) key: {key}");
                AddOpCodeCallback(_opCodeByKey[key], callback);
                return _opCodeByKey[key];
            }
            Debug.Log($"RegisterOpCode not ContainsKey(key) key: {key}");
            await _matchMessageController.SendMatchState(500, JsonConvert.SerializeObject(new OpCodeRegister(key, 0)));
            Debug.Log($"RegisterOpCode SendGameState");
            await UniTask.WaitUntil(() => _opCodeByKey.ContainsKey(key));
            Debug.Log($"RegisterOpCode UniTask.WaitUntil(() => opCodeByKey.ContainsKey(key)");
            AddOpCodeCallback(_opCodeByKey[key], callback);
            return _opCodeByKey[key];
        }
        private void AddOpCodeCallback(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.Log($"addOpCodeCallback opCode: {opCode}");
            if (opCodeCallbacks.ContainsKey(opCode))
            {
                opCodeCallbacks[opCode].AddListener(callback);
            }

            UnityEvent<long, string, IMatchState> newEvent = new UnityEvent<long, string, IMatchState>();
            newEvent.AddListener(callback);
            opCodeCallbacks.TryAdd(opCode, newEvent);
        }
        
        private void RemoveOpCodeCallback(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.Log($"removeOpCodeCallback opCode: {opCode}");
            if (opCodeCallbacks.ContainsKey(opCode))
            {
                opCodeCallbacks[opCode].RemoveListener(callback);
            }
        }
        
        #endregion
        
        
        
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
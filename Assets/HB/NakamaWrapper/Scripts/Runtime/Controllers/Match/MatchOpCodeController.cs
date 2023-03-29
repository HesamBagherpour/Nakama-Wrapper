using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Cysharp.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.Events;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchOpCodeController
    {
        public Dictionary<long, UnityEvent<long, string, IMatchState>> opCodeCallbacks = new Dictionary<long, UnityEvent<long, string, IMatchState>>();
        public Dictionary<string, long> opCodeByKey = new Dictionary<string, long>();
        public Dictionary<long, string> opCodeKeyByValue = new Dictionary<long, string>();

        public MatchMessageController _matchMessageController;

        void init(MatchMessageController matchMessageController)
        {
            _matchMessageController = matchMessageController;
            addOpCodeCallback(500, onReciveRegisterOpCodeResult);
        }
        
        #region OpCodes Logics

        private void addOpCodeCallback(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log($"addOpCodeCallback opCode: {opCode}");
            if (opCodeCallbacks.ContainsKey(opCode))
            {
                opCodeCallbacks[opCode].AddListener(callback);
            }

            UnityEvent<long, string, IMatchState> newEvent = new UnityEvent<long, string, IMatchState>();
            newEvent.AddListener(callback);
            opCodeCallbacks.TryAdd(opCode, newEvent);
        }

        private void removeOpCodeCallback(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log($"removeOpCodeCallback opCode: {opCode}");
            if (opCodeCallbacks.ContainsKey(opCode))
            {
                opCodeCallbacks[opCode].RemoveListener(callback);
            }
        }

        private void onReciveRegisterOpCodeResult(long opCode, string key, IMatchState state)
        {
            Debug.unityLogger.Log($"onReciveRegisterOpCodeResult opCode: {opCode} - data: {Encoding.UTF8.GetString(state.State)}");
            OpCodeRegister opCodeRegister = Encoding.UTF8.GetString(state.State).FromJson<OpCodeRegister>();
            if (!opCodeByKey.ContainsKey(opCodeRegister.key))
                opCodeByKey.TryAdd(opCodeRegister.key, opCodeRegister.opCode);

            if (!opCodeKeyByValue.ContainsKey(opCodeRegister.opCode))
                opCodeKeyByValue.TryAdd(opCodeRegister.opCode, opCodeRegister.key);

            Debug.unityLogger.Log("onReciveRegisterOpCodeResult Init.");
        }

        #endregion
        
        public async UniTask<long> RegisterOpCode(string key, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log("RegisterOpCode start");
            if (opCodeByKey.ContainsKey(key))
            {
                Debug.unityLogger.Log($"RegisterOpCode ContainsKey(key) key: {key}");
                addOpCodeCallback(opCodeByKey[key], callback);
                return opCodeByKey[key];
            }

            Debug.unityLogger.Log($"RegisterOpCode not ContainsKey(key) key: {key}");
            await _matchMessageController.SendMatchState(500, new OpCodeRegister(key, 0).ToJson());
            Debug.unityLogger.Log($"RegisterOpCode SendGameState");
            await UniTask.WaitUntil(() => opCodeByKey.ContainsKey(key));
            Debug.unityLogger.Log($"RegisterOpCode UniTask.WaitUntil(() => opCodeByKey.ContainsKey(key)");
            addOpCodeCallback(opCodeByKey[key], callback);
            return opCodeByKey[key];
        }

        public void UnRegisterOpCode(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log($"UnRegisterOpCode opCode: {opCode}");
            removeOpCodeCallback(opCode, callback);
        }
        
        
    }
    

    
    public class OpCodeRegister
    {
        public string key;
        public long opCode;

        public OpCodeRegister(string key, long opCode)
        {
            this.key = key;
            this.opCode = opCode;
        }
    }
    
    public class OpCodeServerModel
    {
        [DataMember(Name = "opCode")] public long OpCode;
        [DataMember(Name = "key")] public string Key;
    }
}
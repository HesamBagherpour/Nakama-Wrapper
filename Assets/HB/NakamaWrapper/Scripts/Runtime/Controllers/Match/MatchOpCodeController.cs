using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.Events;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Match
{
    public class MatchOpCodeController
    {
        public  Dictionary<long, UnityEvent<long, string, IMatchState>> opCodeCallbacks = new Dictionary<long, UnityEvent<long, string, IMatchState>>();
        private Dictionary<string, long> opCodeByKey = new Dictionary<string, long>();
        public  Dictionary<long, string> opCodeKeyByValue = new Dictionary<long, string>();
        public  MatchMessageController matchMessageController;
        #region Init

            public void Init(MatchMessageController matchMessageController)
            {
                this.matchMessageController = matchMessageController; 
                AddOpCodeCallback(500, ONReciveRegisterOpCodeResult);
            }

        #endregion
        #region OpCodes Logics

        private void AddOpCodeCallback(long opCode, UnityAction<long, string, IMatchState> callback)
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

        private void RemoveOpCodeCallback(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log($"removeOpCodeCallback opCode: {opCode}");
            if (opCodeCallbacks.ContainsKey(opCode))
            {
                opCodeCallbacks[opCode].RemoveListener(callback);
            }
        }

        private void ONReciveRegisterOpCodeResult(long opCode, string key, IMatchState state)
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
        #region Opcode

        public async UniTask<long> RegisterOpCode(string key, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log("RegisterOpCode start");
            if (opCodeByKey.ContainsKey(key))
            {
                Debug.unityLogger.Log($"RegisterOpCode ContainsKey(key) key: {key}");
                AddOpCodeCallback(opCodeByKey[key], callback);
                return opCodeByKey[key];
            }

            Debug.unityLogger.Log($"RegisterOpCode not ContainsKey(key) key: {key}");
            await matchMessageController.SendMatchState(500, new OpCodeRegister(key, 0).ToJson());
            Debug.unityLogger.Log($"RegisterOpCode SendGameState");
            await UniTask.WaitUntil(() => opCodeByKey.ContainsKey(key));
            Debug.unityLogger.Log($"RegisterOpCode UniTask.WaitUntil(() => opCodeByKey.ContainsKey(key)");
            AddOpCodeCallback(opCodeByKey[key], callback);
            return opCodeByKey[key];
        }

        public void UnRegisterOpCode(long opCode, UnityAction<long, string, IMatchState> callback)
        {
            Debug.unityLogger.Log($"UnRegisterOpCode opCode: {opCode}");
            RemoveOpCodeCallback(opCode, callback);
        }

        #endregion
    }
    
}
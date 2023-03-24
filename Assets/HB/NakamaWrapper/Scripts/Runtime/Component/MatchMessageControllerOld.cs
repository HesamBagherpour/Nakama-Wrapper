using System;
using System.Collections.Generic;
using System.Text;
using HB.NakamaWrapper.Scripts.Runtime.Core;
using HB.NakamaWrapper.Scripts.Runtime.Models;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Component
{
    public class MatchMessageControllerOld : MonoBehaviour, I8MatchState
    {
        
        private HSocket _socket;
        public event Action<IMatchState> I8OnReceivedChannelMessage;
        public void Init(HSocket hSocket)
        {
            _socket = hSocket;
            I8OnReceivedChannelMessage += OnOnReceivedMatchMessage;
        }
        private void OnOnReceivedMatchMessage(IMatchState obj)
        {

            Debug.Log("+++++++" + obj.OpCode);
            if (obj.OpCode == 300)
            {
                var stateDictionary = GetStateAsDictionary(obj.State);
                Debug.Log("stateDictionary   :  " + stateDictionary["test"]);
            }

        }
        public void Raise(IMatchState channelMessage)
        {
            I8OnReceivedChannelMessage?.Invoke(channelMessage);
        }
        
        private IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }

    }
}
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Controllers.Channel
{
    public class ChannelMessageController : MonoBehaviour
    {
        private ISocket _socket;
        public void Init(ISocket socket)
        {
            _socket = socket;
            _socket.ReceivedChannelMessage +=SocketOnReceivedChannelMessage;
            _socket.ReceivedChannelPresence +=SocketOnReceivedChannelPresence; 
        }

        private void SocketOnReceivedChannelPresence(IChannelPresenceEvent obj)
        {
            Debug.Log("ChannelId:"+ obj.ChannelId);
            Debug.Log("Message content"+ obj.RoomName);
            Debug.Log("Message content"+ obj.Joins);
            
            AddPresences(obj.Joins);
            RemovePresences(obj.Leaves);
        }

        private void SocketOnReceivedChannelMessage(IApiChannelMessage messageData)
        {
            
            Debug.Log(messageData.Code);
            Debug.Log(messageData.Persistent);
            Debug.Log(messageData.MessageId);
            Debug.Log(messageData.SenderId);
            Debug.Log(messageData.MessageId);
            Debug.Log(messageData.Content);
        }
 
        public async UniTask<bool> SendMessages(string channelId, Dictionary<string, string> messageContent)
        {
            IChannelMessageAck sendAck = await _socket.WriteChatMessageAsync(channelId, messageContent.ToJson());
            return true;
        }

        private void AddPresences(IEnumerable<IUserPresence> userPresence)
        {
            
        }
        private void RemovePresences(IEnumerable<IUserPresence> userPresence)
        {
            
        }
        
    }
    
}
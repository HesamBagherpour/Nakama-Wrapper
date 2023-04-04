using System;
using HB.NakamaWrapper.Scripts.Runtime.Controllers.Channel;
using HB.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using HB.NakamaWrapper.Scripts.Runtime.Controllers.Session;
using HB.NakamaWrapper.Scripts.Runtime.Controllers.Socket;
using HB.NakamaWrapper.Scripts.Runtime.Factory;
using HB.NakamaWrapper.Scripts.Runtime.Manager;
using HB.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;
using Infinite8.NakamaWrapper.Scripts.Runtime.Models;
using UnityEngine;

namespace HB.NakamaWrapper.Scripts.Runtime.Modules.Chat
{
    public class ChatConnection : MonoBehaviour
    {
        // Start is called before the first frame update
        private MatchMessageController _matchMessageController;

        public string clintTag = "aws";
        public string sessionTag = "google";
        public string matchTag = "chat";

        [SerializeField]private ServerClientConfigs serverClientChatConfigs;
        private void Start()
        {
            //ClientConfig clientConfig = new ClientConfig();
            var  uniqueIdentifier = Guid.NewGuid().ToString();
            SessionConfigDevice sessionConfig = new SessionConfigDevice(uniqueIdentifier);
            SocketConfig socketConfig = new SocketConfig();
            RpcConfig rpcConfig = new RpcConfig();
            MatchFactory matchFactory = new MatchFactory();
            ChannelsFactory channelsFactory = new ChannelsFactory();
            MatchConfig matchConfig = new MatchConfig(matchTag);
            Init(clintTag, sessionTag, matchConfig.GETMatchName(), 
                serverClientChatConfigs, sessionConfig, socketConfig,rpcConfig,matchFactory,channelsFactory , matchConfig);
        }
        async void Init<T>(string clientTagName, string sessionTagName, string socketTagName, ServerClientConfigs clientConfig,
            T config, SocketConfig socketConfig, RpcConfig rpcConfig, MatchFactory matchFactory , ChannelsFactory channelsFactory , MatchConfig matchConfig)
            where T : SessionConfig
        {
            
            var (_, client) = await NakamaManager.Instance.ClientFactory.CreateClint(clientTagName, clientConfig);
            var (_, session) = await client.SessionFactory.CreateSession(sessionTagName,client,config);
            var (_, chatSocket) = await session.SocketFactory.CreateSocket(socketTagName, client, socketConfig);
            
            if (TryGetComponent<SessionConnectionController>(out SessionConnectionController sessionConnectionController))
            {
                sessionConnectionController.Init(client,session,true,2,false);
                session.setSessionConnectionController(sessionConnectionController);
                //NakamaManager.Instance.OnSessionConnected?.Invoke(sessionTagName);
            }
            if (TryGetComponent<MatchConnectionController>(out MatchConnectionController matchConnectionController))
            {
                matchConnectionController.Init(chatSocket,matchConfig);
            }
            if (TryGetComponent<ChannelMessageController>(out ChannelMessageController channelConnectionController))
            {
                channelConnectionController.Init(chatSocket.socket);
            
            }
            if (TryGetComponent<SocketConnectionController>(out SocketConnectionController socketConnectionController))
            {
                socketConnectionController.Init(chatSocket);
                await socketConnectionController.ConnectSocket(chatSocket, session, socketConfig);
            }
            // if (TryGetComponent<SocketPingPongConnection>(out SocketPingPongConnection socketPingPongConnection))
            // {
            //     return;
            //     await socketPingPongConnection.Init(chatSocket);
            //     chatSocket.socket.ReceivedMatchState += delegate(IMatchState state)
            //     {
            //         socketPingPongConnection.Raise(state);
            //     };
            //     socketPingPongConnection.StartPingPong();
            // }
            
            var (_, match) = await chatSocket.MatchFactory.CreateMatch(matchTag, client, session, rpcConfig);
            if (TryGetComponent<MatchMessageController>(out MatchMessageController _matchMessageController))
            {
              
                _matchMessageController.Init(chatSocket,match.data.matchId);
                this._matchMessageController = _matchMessageController;
            }
            await matchConnectionController.ConnectMatch(match.data.matchId,this._matchMessageController);
            
            #region test

            //
            // // if (TryGetComponent<OpCodeGenerator>(out OpCodeGenerator opCodeGenerator))
            // // {
            // //     _opCodeGenerator = opCodeGenerator;
            // //     _opCodeGenerator.Init(session.Session.Username,match.data.matchId);
            // //     matchConnectionController.OnMatchConnect?.Invoke(matchConnectionController._matchId);
            // // }
            // //
            //
            // // Debug.unityLogger.Log(await client.client.RpcAsync(session.Session,"test"));
            // //
            // ChannelConfig channelConfig = new ChannelConfig(true ,true,"Chat",ChannelType.Group);
            // var (_, channel) = await channelsFactory.CreateChannel(session.Session.UserId,chatSocket,channelConfig);

            // var content = new Dictionary<string, string> {{"hello", "world"}};
            // await channelConnectionController.SendMessages(channel.Id, content);
            //SendChannelMessage(channel.Id, channelConnectionController);
           
            
            // var opCode = 300;
            // var newState = new Dictionary<string, string> {{"test", "test"}}.ToJson();
            // await chatSocket.socket.SendMatchStateAsync(match.data.matchId,opCode,newState);

            #endregion
        }
        #region Test

        //
        // private async void SendChannelMessage(string channelId , ChannelMessageController channelConnectionController)
        // {
        //     
        //     var content = new Dictionary<string, string> {{"hello", "world"}};
        //     await channelConnectionController.SendMessages(channelId, content);
        // }

        
        
        
                



        #endregion

    }
    
}

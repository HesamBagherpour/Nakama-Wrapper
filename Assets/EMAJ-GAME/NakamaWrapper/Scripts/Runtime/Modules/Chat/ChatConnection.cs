using System;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Channel;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Session;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Socket;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Factory;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Manager;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Models;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.NakamaConfig.ClientConfig;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Modules.Chat
{
    public class ChatConnection : MonoBehaviour
    {
        // Start is called before the first frame update
        public MatchMessageController _matchMessageController;
        public MatchConnectionController _matchConnectionController;

        public string clintTag = "aws";
        public string sessionTag = "google";
        public string matchTag = "chat";

        [SerializeField]private ServerClientConfigs serverClientChatConfigs;
        private void Start()
        {
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
            var (_, client) = await NakamaManager.Instance.ClientFactory.CreateClint(clientTagName, clientConfig); // create client
            var (_, session) = await client.SessionFactory.CreateSession(sessionTagName,client,config); // create session 
            var (_, chatSocket) = await session.SocketFactory.CreateSocket(socketTagName, client, socketConfig); // create  socket 
            if (TryGetComponent<SessionConnectionController>(out SessionConnectionController sessionConnectionController))
            {
                sessionConnectionController.Init(client,session,true,2,false);
                session.setSessionConnectionController(sessionConnectionController);
                session.sessionConnectionController = sessionConnectionController;
            } // setup session Controller 
            if (TryGetComponent<MatchConnectionController>(out MatchConnectionController matchConnectionController))
            {
                matchConnectionController.Init(chatSocket,matchConfig);
            }// setup Match  Controller 
            if (TryGetComponent<ChannelMessageController>(out ChannelMessageController channelConnectionController))
            {
                channelConnectionController.Init(chatSocket.socket);
            
            }// setup channel  Controller 
            if (TryGetComponent<SocketConnectionController>(out SocketConnectionController socketConnectionController))
            {
                socketConnectionController.Init(client,session ,chatSocket);
                await socketConnectionController.ConnectSocket(chatSocket, session, socketConfig);
            }// setup socket  Controller 
            var (_, match) = await chatSocket.MatchFactory.CreateMatch(matchTag,client,session,rpcConfig,_matchMessageController,_matchConnectionController);// create Match
            if (TryGetComponent<MatchMessageController>(out MatchMessageController matchMessageController))
            {
                matchMessageController.Init(chatSocket,match.data.matchId);
            }
            await matchConnectionController.ConnectMatch(match.data.matchId,this._matchMessageController);
            
            
            
            #region test
            
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
    
}

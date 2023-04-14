using Emaj_Game.NakamaWrapper.Scripts.Runtime.Controllers.Match;
using Emaj_Game.NakamaWrapper.Scripts.Runtime.Core;
using UnityEngine;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Core
{
    public class EM_Match
    {
        public EM_Socket socket;
        public EM_Client client;
        public EM_Session session;
        public string tag;
        public string matchId;
        public MatchMessageController matchMessageController;
        public MatchConnectionController matchConnectionController;
        
        public EM_Match()
        {
        }

        public EM_Match(string tag, string matchId, EM_Client client, EM_Session session, MatchMessageController matchMessage , MatchConnectionController  matchConnection) {

            this.client = client;
            this.session = session;
            this.tag = tag;
            this.matchId = matchId;
            matchMessageController = matchMessage;
            matchConnectionController = matchConnection;


        }
        public EM_Match(string tag, string matchId, EM_Client client, EM_Session session) {

            this.client=client;
            this.session=session;
            this.tag=tag;
            this.matchId=matchId;
        }
    }
}
using Infinite8.NakamaWrapper.Scripts.Runtime.Core;
using Nakama;

namespace Emaj_Game.NakamaWrapper.Scripts.Runtime.Core
{
    public class EM_MatchmakerTicket
    {
        public EM_Socket socket;
        public EM_Client client;
        public EM_Session session;
        public IMatchmakerTicket MatchmakerTicket;
        public string tag;
        public string matchId;

        public EM_MatchmakerTicket()
        {
        }

        public EM_MatchmakerTicket(EM_Socket socket, EM_Client client, EM_Session session, IMatchmakerTicket matchmakerTicket, string tag, string matchId)
        {
            this.socket = socket;
            this.client = client;
            this.session = session;
            MatchmakerTicket = matchmakerTicket;
            this.tag = tag;
            this.matchId = matchId;
        }
    }
}
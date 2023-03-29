namespace HB.NakamaWrapper.Scripts.Runtime.Core
{
    public class I8Match
    {
        public I8Socket socket;
        public I8Client client;
        public I8Session session;
        public string tag;
        public string matchId;
        
        public I8Match()
        {
        }

        public I8Match(string tag, string matchId, I8Client client, I8Session session) {

            this.client = client;
            this.session = session;
            this.tag = tag;
            this.matchId = matchId;
        }

    }
}
using System.Collections.Generic;
using System.Text;
using Nakama.TinyJson;

namespace HB.NakamaWrapper.Scripts.Runtime.Utilities
{
    public static class GetState
    {
     
        public static IDictionary<string, string> GetStateAsDictionary(byte[] state)
        {
            return Encoding.UTF8.GetString(state).FromJson<Dictionary<string, string>>();
        }
    }
}
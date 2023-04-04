using System;
using UnityEngine;
using System.Text.Json;
using Palmmedia.ReportGenerator.Core.Common;

namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public class GeneralResModel<T>
    {
        public string error;
        public T data;

        public GeneralResModel()
        {
        }

        public GeneralResModel(string error)
        {
            this.error = error;
        }

        public GeneralResModel(T data)
        {
            this.data = data;
        }
    }
    
    [Serializable]
    public class BaseReqModel
    {
        public Action cancel;
    }
    [Serializable]
    public class MatchData{
        public string matchId { get; set; }
    }

    
}
using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Infinite8.NakamaWrapper.Scripts.Runtime.Models
{
    [Serializable]
    public class MoveStateModelNew
    {
        public float x;
        public float y;
        public float z;
        public Vector3 pos;

        public MoveStateModelNew(float x, float y, float z, Vector3 pos)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.pos = pos;
        }
    }
}
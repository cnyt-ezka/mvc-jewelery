using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public class PoolHelperVO 
    {
        public PoolKey Key;
        public int Count;
        public GameObject Prefab;
    }
}
using Runtime.Data.UnityObject;
using Runtime.Enums.Source;
using UnityEngine;

namespace Runtime.Models
{
    /// <summary>
    /// game related datas
    /// </summary>
    public interface IGameModel
    {
        RD_GameStatus Status { get; }
        RD_PoolHelper PoolHelper { get; }
        string GetPoolKey(AssetID id);
        Transform SourceContainer { get; set; }
        Vector3 SourceContainerStartPos { get; set; }
        public bool IsFirstSaleCompleted{ get; set; }
        void Clear();
    }
}
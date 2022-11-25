using Runtime.Data.UnityObject;
using Runtime.Data.UnityObject.Source;
using Runtime.Enums.Source;
using UnityEngine;

namespace Runtime.Models
{
    public class GameModel : IGameModel
    {
        private RD_GameStatus _status;
        private RD_PoolHelper _poolHelper;
        private CD_Assets _assets;
        public RD_GameStatus Status => _status;
        public RD_PoolHelper PoolHelper => _poolHelper;
        public Transform SourceContainer { get; set; } = null;
        public Vector3 SourceContainerStartPos { get; set; }

        private bool _isFirstSaleCompleted;
        public bool IsFirstSaleCompleted
        {
            get => _isFirstSaleCompleted;
            set => _isFirstSaleCompleted = value;
        }

        [PostConstruct]
        public void OnPostConstruct()
        {
            _poolHelper = Resources.Load<RD_PoolHelper>("Data/PoolHelper");
            _status = Resources.Load<RD_GameStatus>("Data/GameStatus");
            _assets = Resources.Load<CD_Assets>("Data/AssetKeyMap");
            Clear();
        }

        public string GetPoolKey(AssetID id)
        {
            return _assets.Map[id].ToString();
        }

        public void Clear()
        {
            _status.Value = 0;
        }

    }
}
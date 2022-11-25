using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Models
{
    public class LevelModel : ILevelModel
    {
        private CD_Levels _levels;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _levels = Resources.Load<CD_Levels>("Data/Levels");
        }

        public LevelVO GetLevel(int levelIndex)
        {
            return _levels.Data[(levelIndex-1) % _levels.Data.Count];
        }

    }
}

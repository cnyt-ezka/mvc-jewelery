using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Config Data/Level List", order = 1)]
    public class CD_Levels : SafeSerializedScriptableObject
    {
        [ListDrawerSettings(ShowIndexLabels = true, AddCopiesLastElement = true, ListElementLabelName = "LevelName")]
        [PropertySpace(SpaceAfter = 50)]
        public List<LevelVO> Data;
    }
}

using System.Collections.Generic;
using Runtime.Data.ValueObject.Source;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public class LevelVO : BaseVO
    {
        public string LevelName;
        public string EnvironmentScene;
        public Vector3 SourceContainerStartPos;
        
        public MainCharacterVO MainCharacter = new();
        
        [BoxGroup("Processors")][GUIColor(.5f,.7f,1f)]
        [ListDrawerSettings(ListElementLabelName = nameof(ActivationAreaVO.ID))]
        public List<ActivationAreaVO> ActivationAreas = new();

        [BoxGroup("Processors")][GUIColor(1f,1f,.5f)]
        [ListDrawerSettings(ListElementLabelName = nameof(ProcessorAreaVO.ID))]
        public List<ProcessorAreaVO> ProcessorAreas = new();
    }
}

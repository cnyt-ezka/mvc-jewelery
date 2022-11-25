using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable]
    public class AlignmentVO : BaseVO
    {
        [FoldoutGroup("Alignment Properties")]
        public Vector2 Size = Vector2.one;

        [FoldoutGroup("Alignment Properties")] 
        public Vector2 GridCount = Vector2.one;
        
        [FoldoutGroup("Alignment Properties")]
        public float HeightInterval = .1f;
        
        [FoldoutGroup("Alignment Properties")]
        public Vector3 Rotation = Vector3.zero;
        
        [FoldoutGroup("Alignment Properties")]
        public Vector3 Scale = Vector3.one;
    }
}
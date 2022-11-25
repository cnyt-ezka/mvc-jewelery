using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable]
    public class TransformVO : BaseVO
    {
        [DisableInEditorMode] [ES3NonSerializable] [FoldoutGroup(nameof(TransformVO))] [PropertyOrder(100)]
        public GameObject Obj;

        [FoldoutGroup(nameof(TransformVO))] [PropertyOrder(100)]
        public Vector3 Position;

        [FoldoutGroup(nameof(TransformVO))] [PropertyOrder(100)]
        public Vector3 Rotation;
        
        [FoldoutGroup(nameof(TransformVO))] [PropertyOrder(100)]
        public Vector3 Scale = Vector3.one;
    }
}

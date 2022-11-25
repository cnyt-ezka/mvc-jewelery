using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class AreaVO : TransformVO
    {
        [PropertyOrder(-2)] public AssetID ID;
        [PropertyOrder(-1)][FoldoutGroup("Area Properties")] public bool IsVisible;
        [PropertyOrder(-1)][FoldoutGroup("Area Properties")] public bool IsInstalled;
        [PropertyOrder(-1)][FoldoutGroup("Area Properties")] [HideIf(nameof(IsInstalled))]
        public ConsumeVO InstallationCost;
        [PropertyOrder(-1)][FoldoutGroup("Area Properties")] [HideIf(nameof(IsInstalled))]
        public List<AssetID> InstallationResult = new ();
        
    }
}
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class ActivationAreaVO : AreaVO
    {
        [FoldoutGroup("ActivationArea Properties")]
        public ActivationVO Activation;
    }
}
using Runtime.Enums;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class StackableItemVO : TransformVO
    {
        public StackableType Type;
        public int Amount = 1;
    }
}
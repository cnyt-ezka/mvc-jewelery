using Runtime.Enums.Source;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class ConsumeVO : BaseVO
    {
        public StackableType Type;
        
        public int MaxAmount = 10;
        
        [ShowIf(nameof(IsRuntimeData))]
        public int CurrentAmount = 0;
        
        public float Time = 1;
        
        #region Helper Methods
        public bool Check(StackableType stackableType)
        {
            return Type.HasFlag(stackableType);
        }
        public bool IsMax()
        {
            return CurrentAmount >= MaxAmount;
        }
        
        #endregion
    }
}
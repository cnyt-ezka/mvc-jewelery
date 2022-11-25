using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class ActivationVO : BaseVO
    {
        public ActivationType Type;
        [ShowIf(nameof(Type),ActivationType.Time)]
        public float Time = 1;

        [ShowIf(nameof(Type),ActivationType.Consume)]
        public int MaxAmount = 100;
        
        [ShowIf(nameof(Type),ActivationType.Consume)]
        [ShowIf(nameof(IsRuntimeData))]
        public int CurrentAmount = 0;
        
        public bool IsLoop;

        #region Helper Methods
        public bool IsMax()
        {
            return CurrentAmount >= MaxAmount;
        }
        
        #endregion
    }

    public enum ActivationType
    {
        Time,
        Consume,
    }
}
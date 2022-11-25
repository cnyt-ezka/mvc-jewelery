using Runtime.Enums.Source;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable]
    public class ProcessorVO
    {
        public ProcessorType Type;
        public float TimeInterval = 0.1f;
    }
}
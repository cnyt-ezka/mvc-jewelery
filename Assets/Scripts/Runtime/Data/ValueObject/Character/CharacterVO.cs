using Runtime.Data.ValueObject.Source;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Character
{
    [System.Serializable]
    public class CharacterVO : TransformVO
    {
        [HideIf(nameof(IsRuntimeData))]
        public AssetID ID;

        public CharacterMovementVO Movement;
        //public CharacterHealthVO Health;
    }
}
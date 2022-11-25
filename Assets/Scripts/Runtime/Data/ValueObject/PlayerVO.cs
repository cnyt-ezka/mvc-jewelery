using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public class PlayerVO
    {
        public CurrencyVO Currency;
        public bool IsNewLevel = true; 
        public int CurrentLevelIndex;

        [ShowInInspector] public LevelVO Level = new ();
    }
}

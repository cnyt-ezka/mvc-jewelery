namespace Runtime.Data.ValueObject.Character
{
    [System.Serializable]
    public class CharacterHealthVO
    {
        public int MaxHealth = 1;
        public int CurrentHealth = 1;
        public bool IsDead => CurrentHealth <= 0;
    }
}
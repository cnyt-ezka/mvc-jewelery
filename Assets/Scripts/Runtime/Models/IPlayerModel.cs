using Runtime.Data.ValueObject;

namespace Runtime.Models
{
    public interface IPlayerModel
    {
        bool IsNewLevel { get; }
        LevelVO GetLevel();
        void SaveLevel(LevelVO vo);
        int SetNextLevelIndex();
        
        int GetCurrency();
        void AddCurrency(int amount);
        bool RemoveCurrency(int amount);
        void SetCurrency(int amount);
        int GetLevelIndex();
        public void SetNewLevel();
        void SetCurrentLevel(int index);
    }
}

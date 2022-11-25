using Runtime.Data.ValueObject;

namespace Runtime.Models
{
    public interface ILevelModel
    {
        LevelVO GetLevel(int levelIndex);
    }
}

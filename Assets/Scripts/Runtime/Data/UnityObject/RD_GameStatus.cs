using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Game Status", order = 1)]
    public class RD_GameStatus : ScriptableObject
    {
        public GameStatus Value;
        public void Block()
        {
            Value |= GameStatus.Blocked;
        }

        public void UnBlock()
        {
            Value &= ~GameStatus.Blocked;
        }

        public void Reset()
        {
            Value = GameStatus.None;
        }
    }
}
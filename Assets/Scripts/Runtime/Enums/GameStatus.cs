using System;

namespace Runtime.Enums
{
    [Flags]
    public enum GameStatus
    {
        None = 0,
        Blocked = 1 << 0,
        Test1 = 1 << 1,
        Test2 = 1 << 2,
        Test = Test1 | Test2,
        All = ~(-1 << 3)
    }
}
using System;

namespace Runtime.Enums.Source
{
    [Flags]
    public enum StackableType
    {
        None = 0,
        GreenStone = 1 << 0,
        GreenGem = 1 << 1,
        GreenJewel = 1 << 2,
        
        MagentaStone = 1 << 10,
        MagentaGem = 1 << 11,
        MagentaJewel = 1 << 12,
        MagentaPolished = 1 << 13,
        
        Money = 1 << 19,
        
        All = ~(-1 << 20)
    }
}
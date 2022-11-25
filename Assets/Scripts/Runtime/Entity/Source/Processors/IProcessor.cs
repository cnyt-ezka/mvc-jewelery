using System.Collections.Generic;
using Runtime.Data.ValueObject.Source;
using Runtime.Enums.Source;
using UnityEngine;

namespace Runtime.Entity.Source.Processors
{
    public interface IProcessor
    {
        ProcessorType Type { get; }
        StackableType GetItemType();
        Transform Transform { get; }
        public abstract bool AddItem(StackableItemVO newItem);
        public abstract bool RemoveItem(out StackableItemVO removedItem);
        public abstract bool RemoveItems(out List<StackableItemVO> removedItems);
    }
}
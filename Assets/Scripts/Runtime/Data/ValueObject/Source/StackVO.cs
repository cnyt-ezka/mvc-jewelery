using System.Collections.Generic;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class StackVO : BaseVO
    {
        public StackableType Type;
        
        public int MaxCount = 10;
        
        public float TimeInterval = 1f;

        [HideLabel]
        public AlignmentVO Alignment;
        
        [ShowIf("IsRuntimeData")]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "Type")] 
        public List<StackableItemVO> StackableItems = new();

        [HideInInspector]
        public int TempStackCount = 0;

        #region Helper Methods

        
        public void AddItem(StackableItemVO item)
        {
            StackableItems.Add(item);
        }

        public void RemoveItem(StackableItemVO item)
        {
            StackableItems.Remove(item);
        }
        
        public bool Check(StackableType stackableType)
        {
            return Type.HasFlag(stackableType);
        }
        
        public bool IsFull()
        {
            return TempStackCount >= MaxCount;
        }
        
        public StackableType GetItemType()
        {
            return Type;
        }
        
        public StackableItemVO GetItem()
        {
            if (StackableItems.Count == 0)
                return null;
            
            return StackableItems[^1];
        }
        

        #endregion
    }
}
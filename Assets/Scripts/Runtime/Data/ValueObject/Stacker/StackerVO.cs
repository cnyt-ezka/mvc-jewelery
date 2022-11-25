using System.Collections.Generic;
using System.Linq;
using Runtime.Data.ValueObject.Source;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Stacker
{
    [System.Serializable][HideReferenceObjectPicker]
    public class StackerVO : BaseVO
    {
        [ShowIf(nameof(IsRuntimeData))]
        public int CurrentGradeIndex = 0;
        
        public List<StackVO> StackGrades = new() {new StackVO()};

        #region Helper Methods

        public StackVO CurrentStack => StackGrades[CurrentGradeIndex];

        public void Add(StackableItemVO vo)
        {
            CurrentStack.AddItem(vo);
        }

        public void Remove(StackableItemVO vo)
        {
            CurrentStack.RemoveItem(vo);
        }
        
        public int GetMaxCapacity()
        {
            return CurrentStack.MaxCount;
        }
        
        public bool IsFull()
        {
            return CurrentStack.TempStackCount >= CurrentStack.MaxCount;
        }

        public bool IsPossibleToInteract(StackableType type)
        {
            return CurrentStack.Check(type);
        }

        public StackableItemVO GetStackableItemToExport(StackableType stackableType)
        {
            if (stackableType == StackableType.None)
                return null;
            
            var searchList = new List<StackableItemVO>(CurrentStack.StackableItems);
            for (int ii = searchList.Count - 1; ii >= 0; ii--)
            {
                var searchItem = searchList[ii];

                if (stackableType.HasFlag(searchItem.Type))
                    return searchItem;
            }

            return null;
        }

        public StackableItemVO GetStackableItem(StackableType targetItemType)
        {
            return CurrentStack.StackableItems.FirstOrDefault(x => x.Type == targetItemType);
        }
        
        #endregion

    }
}
using System;
using System.Collections.Generic;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.StackableItems;
using UnityEngine;

namespace Runtime.Entity.Source.Processors.Stackable
{
    public class SourceProcessor : StackableProcessor
    {
        public Action SourceAdded { get; private set; }
        public int PossibleRemoveItemCount { get; set; }

        public override void Setup(StackVO vo, Action sourceAddedCallback)
        {
            base.Setup(vo, sourceAddedCallback);

            SourceAdded = sourceAddedCallback;
        }

        public override void OnProcessStarted()
        {
            _vo.TempStackCount++;
        }

        public override void OnProcessFinished()
        {
            AlignStack();
            
            SourceAdded?.Invoke();
        }

        public override bool AddItem(StackableItemVO newItem)
        {
            if (_vo.IsFull())
                return false;

            OnProcessStarted();

            var newStackableItem = newItem.Obj.GetComponent<StackableItem>();
            newStackableItem.MoveToDynamicTarget(_stackHolder, Vector3.zero, OnMoveToStackHolderCompleted);

            return true;
        }

        public override bool RemoveItem(out StackableItemVO removedItem)
        {
            removedItem = null;
            return false;
        }

        public override bool RemoveItems(out List<StackableItemVO> willRemoveItems)
        {
            willRemoveItems = new List<StackableItemVO>();

            if (PossibleRemoveItemCount == 0)
                return false;
            
            for (int ii = PossibleRemoveItemCount - 1; ii >= 0; ii--)
            {
                willRemoveItems.Add(_vo.StackableItems[ii]);
                _vo.RemoveItem(_vo.StackableItems[ii]);;
                _vo.TempStackCount--;
            }
            
            AlignStack();

            return true;
        }

        private void OnMoveToStackHolderCompleted(StackableItem stackableItem)
        {
            _vo.AddItem(stackableItem.VO);
            
            OnProcessFinished();
        }

        public List<StackableItemVO> GetItems()
        {
            return _vo.StackableItems;
        }
    }
}
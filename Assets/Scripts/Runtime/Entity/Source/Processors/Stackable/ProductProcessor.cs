using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Model;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.UI;
using UnityEngine;

namespace Runtime.Entity.Source.Processors.Stackable
{
    public class ProductProcessor : StackableProcessor
    {
        public Action ProductRemoved { get; set; }
        public Action ProductAdded { get; set; }

        [SerializeField] 
        private ProductProcessorUI _productProcessorUI;

        public override void Setup(StackVO vo, IPoolModel pool, Action callback)
        {
            base.Setup(vo, pool, callback);

            ProductRemoved = callback;
            
            ProductAdded += _productProcessorUI.UpdateUI;
            ProductRemoved += _productProcessorUI.UpdateUI;

            _productProcessorUI.Setup(_vo);
        }

        public override void OnProcessStarted()
        {
            _vo.TempStackCount++;
        }

        public override void OnProcessFinished()
        {
            ProductAdded?.Invoke();
        }

        public override bool AddItem(StackableItemVO newProduct)
        {
            _vo.AddItem(newProduct);
            OnProcessFinished();

            return true;
        }

        public override bool RemoveItem(out StackableItemVO removedItem)
        {
            removedItem = _vo.GetItem();
            if (removedItem == null)
                return false;

            _vo.RemoveItem(removedItem);
            
            _vo.TempStackCount--;
            ProductRemoved?.Invoke();

            return true;
        }
        public bool RemoveItem(StackableItemVO item)
        {
            if (item == null)
                return false;
            
            _vo.RemoveItem(item);
            
            _vo.TempStackCount--;
            ProductRemoved?.Invoke();

            return true;
        }

        public override bool RemoveItems(out List<StackableItemVO> removedItems)
        {
            removedItems = null;
            return false;
        }
    }
}
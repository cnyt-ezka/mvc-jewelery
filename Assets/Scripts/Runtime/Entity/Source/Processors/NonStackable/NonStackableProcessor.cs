using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Model;
using Runtime.Data.ValueObject.Source;
using Runtime.Enums.Source;
using UnityEngine;

namespace Runtime.Entity.Source.Processors.NonStackable
{
    public abstract class NonStackableProcessor : MonoBehaviour, IProcessor
    {
        [SerializeField]
        protected ProcessorVO _configVO;
        protected ConsumeVO _vo;
        public Transform Transform => transform;
        public abstract void Setup(ConsumeVO vo, IPoolModel pool, Action callback);
        public virtual void DestroyProcessor()
        {
            
        }
        
        #region IProcessor Methods

        public ProcessorType Type => _configVO.Type;
        public StackableType GetItemType()
        {
            return _vo.Type;
        }
        public abstract bool AddItem(StackableItemVO newItemVO);
        public abstract bool RemoveItem(out StackableItemVO removedItemVO);
        public abstract bool RemoveItems(out List<StackableItemVO> removedItems);

        #endregion
    }
}
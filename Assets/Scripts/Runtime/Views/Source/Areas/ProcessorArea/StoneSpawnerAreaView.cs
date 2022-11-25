using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.StackableItems;
using Runtime.Views.Source.AreaBase;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class StoneSpawnerAreaView : ProcessorAreaView
    {
        private ProcessorAreaVO _vo => VO as ProcessorAreaVO;
        protected override void Produce(Action productionCompletedCallback, List<StackableItemVO> consumedItems = null)
        {
            var alignPoint =_productProcessor.GetLastAlignPoint();
            var item = Pool.Get(_productProcessor.GetItemType().ToString(), alignPoint).GetComponent<StackableItem>();
            
            item.Setup(new StackableItemVO()
            {
                Amount = item.VO.Amount,
                Type = _productProcessor.GetItemType(),
                IsRuntimeData = true
            });
            
            _productProcessor.AddItem(item.VO);
            _productProcessor.AlignStack();
            
            DOVirtual.DelayedCall(_productProcessor.GetTimeInterval(), ()=> productionCompletedCallback?.Invoke());
        }

    }
}
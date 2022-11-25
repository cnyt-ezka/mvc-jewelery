using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source;
using Runtime.Entity.Source.StackableItems;
using Runtime.Views.Source.AreaBase;
using UnityEngine;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class MoneyAreaView : ProcessorAreaView
    {
        private ProcessorAreaVO _vo => VO as ProcessorAreaVO;

        protected override void Produce(Action productionCompletedCallback, List<StackableItemVO> consumedItems = null)
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                for (int ii = 0; ii < _vo.GetProductStack().MaxCount; ii++)
                {
                    var alignPoint =_productProcessor.GetLastAlignPoint();
                    var item = Pool.Get(_productProcessor.GetItemType().ToString(), alignPoint).GetComponent<StackableItem>();
                    item.VO.Obj = item.gameObject;
                    
                    _productProcessor.AddItem(item.VO);
                }
            });
        }

        protected override void OnProductRemoved()
        {
            base.OnProductRemoved();

            if (_vo.GetProductStack().StackableItems.Count == 0)
            {
                DOVirtual.DelayedCall(2f, () =>
                {
                    _areaHolder.gameObject.SetActive(true);
                    _areaHolder.DOScale(Vector3.zero, .5f)
                        .SetEase(Ease.InBack).
                        OnComplete(() =>
                        {
                            VO.IsInstalled = false;
                            VO.IsVisible = false;
                            
                            Destroy(VO.Obj);
                        });
                });
            }
        }
    }
}

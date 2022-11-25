using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.StackableItems;
using Runtime.Views.Source.AreaBase;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class GateView : ProcessorAreaView
    {
        private ProcessorAreaVO _vo => VO as ProcessorAreaVO;

        private StackableItem _consumeItem;
        
        public Action SpeedUpgraded { get; set; }

        protected override void Produce(Action productionCompletedCallback, List<StackableItemVO> consumedItems = null)
        {
          
        }

        protected override void OnUpgradeCompleted()
        {
            base.OnUpgradeCompleted();

            _upgradeProcessor.transform.DOMoveY(_upgradeProcessor.transform.position.y - 5f, 1f); //.SetActive(false);
        }
    }
}
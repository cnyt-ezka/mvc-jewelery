using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.StackableItems;
using Runtime.Models;
using Runtime.Views.Source.AreaBase;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class RingAreaView : ProcessorAreaView
    {
        private ProcessorAreaVO _vo => VO as ProcessorAreaVO;
        public Action BlockStatus { get; set; }
        public Action UnBlockStatus { get; set; }
        public Action<int> EarnMoney { get; set; }

        [SerializeField] private Transform _productionPoint;
        [SerializeField] private PlayableDirector _timelineDirecrtor;
        [SerializeField] private TimelineAsset _timelineAsset;
        [SerializeField] private TimelineAsset _timelineAssetFree;
        private StackableItem _consumeItem;
        [Inject] private IGameModel _game { get; set; }
        protected override void Produce(Action productionCompletedCallback, List<StackableItemVO> consumedItems = null)
        {
            if (consumedItems == null)
                return;

            ProduceWithPlayer(consumedItems[0]);
        }

        private void ProduceWithPlayer(StackableItemVO item)
        {
            if (!_game.IsFirstSaleCompleted)
                BlockStatus?.Invoke();
            else
                _timelineDirecrtor.playableAsset = _timelineAssetFree;

            _consumeItem = item.Obj.GetComponent<StackableItem>();
            
            _timelineDirecrtor.time = 0;
            _timelineDirecrtor.Play();
        
            Debug.Log("AnimationStart +++ ");

            _consumeItem.MoveToDynamicTarget(_productionPoint, Vector3.zero, JewelMoveCompleted, 1/2.5f);
            
            DOVirtual.DelayedCall((float)_timelineDirecrtor.duration, () =>
            {
                if (!_game.IsFirstSaleCompleted)
                    UnBlockStatus?.Invoke();
                
                if (_productProcessor.RemoveItem(_consumeItem.VO))
                {
                    _consumeItem.transform.SetParent(null);
                    _consumeItem.gameObject.SetActive(true);
                    Pool.Return(_consumeItem.gameObject);
                    EarnMoney?.Invoke(_consumeItem.VO.Amount);
                    ProductionCompletedCallback?.Invoke();
                }
            });
        }

        private void JewelMoveCompleted(StackableItem item)
        {
            //Pool.Return(item.gameObject);
        }

        public void SwitchTimelineAsset()
        {
            _timelineDirecrtor.playableAsset = _timelineAssetFree;
        }

    }
}
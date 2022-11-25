using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.StackableItems;
using Runtime.Enums.Source;
using Runtime.Views.Source.AreaBase;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class LaserAreaView : ProcessorAreaView
    {
        private ProcessorAreaVO _vo => VO as ProcessorAreaVO;
        public Action BlockStatus { get; set; }
        public Action UnBlockStatus { get; set; }
        public Action<Transform, float, Action> MovePlayerForTimelineAnim { get; set; }
        public Action<StackableItem> SendProductToPlayerStack { get; set; }

        [SerializeField] private Transform _productionPoint;
        [SerializeField] private Transform _animationCharacterHolder;
        [SerializeField] private Transform _productCollider;
        [SerializeField] private PlayableDirector _timelineDirecrtor;
        [SerializeField] private TimelineAsset _timelineAsset;
        [SerializeField] private TimelineAsset _timelineAssetUpgraded;
        [SerializeField] private Transform _greenGemMesh;
        [SerializeField] private Transform _magentaGemMesh;
        private Transform _consumeItem;

        protected override void Produce(Action productionCompletedCallback, List<StackableItemVO> consumedItems = null)
        {
            Debug.Log("LaserAreaView produce +++");
            if (consumedItems == null)
                return;
            
            if (_productProcessor.GetItemType() == StackableType.GreenJewel)
            {
                _greenGemMesh.gameObject.SetActive(true);
                _magentaGemMesh.gameObject.SetActive(false);
            }
            else
            {
                _greenGemMesh.gameObject.SetActive(false);
                _magentaGemMesh.gameObject.SetActive(true);
            }
            
            if (_vo.CurrentGradeIndex == 0)
                ProduceWithPlayer(consumedItems[0].Obj.transform);
            else
                ProduceWithAssistant(consumedItems[0].Obj.transform);
        }

        protected override void AreaStart()
        {
            base.AreaStart();
            
            _animationCharacterHolder.gameObject.SetActive(false);
            
            if (_vo.CurrentGradeIndex == 0)
                _productCollider.gameObject.SetActive(false);
            else
                _productCollider.gameObject.SetActive(true);
            
        }

        private void ProduceWithPlayer(Transform item)
        {
            BlockStatus?.Invoke();

            MovePlayerForTimelineAnim?.Invoke(transform, (float)_timelineDirecrtor.duration, AnimationPlayerStart);
            
            _consumeItem = item;
        }

        private void AnimationPlayerStart()
        {
            _timelineDirecrtor.playableAsset = _timelineAsset;
            _timelineDirecrtor.time = 0;
            _timelineDirecrtor.Play();
            
            var product = Pool.Get(_productProcessor.GetItemType().ToString(), _productionPoint).GetComponent<StackableItem>();
            product.Setup(new StackableItemVO()
            {
                Amount = product.VO.Amount,
                IsRuntimeData = true,
                Type = _productProcessor.GetItemType()
            });
            product.transform.localScale = Vector3.zero;
            product.gameObject.SetActive(false);
            
            _consumeItem.DORotate(Vector3.zero, .1f);
            _consumeItem.DOMove(_productionPoint.position, .2f).OnComplete(()=>
            {
                Pool.Return(_consumeItem.gameObject);
                product.gameObject.SetActive(true);
                product.transform.DOScale(Vector3.one, .75f).SetDelay(1f);
            }).SetDelay(1.7f);
            
            _animationCharacterHolder.gameObject.SetActive(true);

            DOVirtual.DelayedCall((float)_timelineDirecrtor.duration, () =>
            {
                _animationCharacterHolder.gameObject.SetActive(false);
                UnBlockStatus?.Invoke();
                if (_productProcessor.RemoveItem(product.VO))
                {
                    SendProductToPlayerStack?.Invoke(product);
                    ProductionCompletedCallback?.Invoke();
                }
            });
        }

        private void ProduceWithAssistant(Transform item)
        {
            _consumeItem = item;
            
            _timelineDirecrtor.playableAsset = _timelineAssetUpgraded;
            _timelineDirecrtor.time = 0;
            _timelineDirecrtor.Play();

            AnimationAssistantStart();
        }

        private void AnimationAssistantStart()
        {
            _animationCharacterHolder.gameObject.SetActive(false);
            
            var product = Pool.Get(_productProcessor.GetItemType().ToString(), _productionPoint).GetComponent<StackableItem>();
            product.Setup(new StackableItemVO()
            {
                Amount = product.VO.Amount,
                IsRuntimeData = true,
                Type = _productProcessor.GetItemType()
            });
            product.transform.localScale = Vector3.zero;
            product.gameObject.SetActive(false);
            
            _consumeItem.DORotate(Vector3.zero, .1f);
            _consumeItem.DOMove(_productionPoint.position, .2f).OnComplete(()=>
            {
                Pool.Return(_consumeItem.gameObject);
                product.gameObject.SetActive(true);
                product.transform.DOScale(Vector3.one, .75f).SetDelay(1f);
            }).SetDelay(1.7f);

            DOVirtual.DelayedCall((float)_timelineDirecrtor.duration, () =>
            {
                var parent =_productProcessor.GetLastAlignPoint();
                
                product.transform.SetParent(parent);
                product.transform.DOLocalMove(Vector3.zero, .2f);
                product.transform.DOLocalRotate(Vector3.zero, .2f);
                product.transform.DOScale(Vector3.one, .2f);
                
                _productProcessor.AddItem(product.VO);
                ProductionCompletedCallback?.Invoke();
            });
        }
        protected override void OnUpgradeCompleted()
        {
            _vo.CurrentGradeIndex++;

            SetupSourceProcessors();
            SetupProductProcessor();
            
            _productCollider.gameObject.SetActive(true);

            _upgradeProcessor.gameObject.SetActive(false);
        }
    }
}
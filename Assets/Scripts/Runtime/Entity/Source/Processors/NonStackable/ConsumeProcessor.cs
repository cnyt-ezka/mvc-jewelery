using System;
using System.Collections.Generic;
using DG.Tweening;
using MVC.Base.Runtime.Abstract.Model;
using Runtime.Data.ValueObject.Source;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace Runtime.Entity.Source.Processors.NonStackable
{
    public class ConsumeProcessor : NonStackableProcessor
    {
        public Action Completed { get; set; }
        
        [SerializeField]
        protected Transform _consumeTarget;
        [SerializeField]
        protected TextMeshProUGUI _text;
        [SerializeField] 
        protected ProceduralImage _fillImage;

        private List<Transform> Items = new ();

        private Tween _imageFillAmountTween;
        private IPoolModel _pool;
        private float _lastPendingTime;

        public override void Setup(ConsumeVO vo, IPoolModel pool, Action callback)
        {
            if (vo == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            
            _vo = vo;
            _pool = pool;

            UpdateText();
            Completed = callback;
        }

        public virtual void SetMax()
        {
            _text.text = "MAX";
            
            if (!_imageFillAmountTween.IsActive())
                _fillImage.fillAmount = 1;
            
        }

        protected void UpdateText()
        {
            _text.text = (_vo.MaxAmount - _vo.CurrentAmount).ToString();

            float startValue = _fillImage.fillAmount;
            
            _imageFillAmountTween?.Kill();
            _imageFillAmountTween = DOVirtual.Float(startValue, (float)_vo.CurrentAmount / _vo.MaxAmount, 0.2f, (float updatedValue) =>
            {
                _fillImage.fillAmount = updatedValue;
            });
        }

        public bool IsBusy()
        {
            return Time.time < _lastPendingTime + _configVO.TimeInterval;
        }
        
        public bool IsMax()
        {
            return _vo.IsMax();
        }
        
        public override bool AddItem(StackableItemVO newItem)
        {
            if (IsBusy())
                return false;

            _lastPendingTime = Time.time;
            
            _vo.CurrentAmount++;
            Items.Add(newItem.Obj.transform);
            newItem.Obj.transform.DOScale(Vector3.one * .3f, .5f)
                .SetEase(Ease.InOutQuad);
            newItem.Obj.transform.DOMove(_consumeTarget.position, .5f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    Items.Remove(newItem.Obj.transform);
                    _pool.Return(newItem.Obj);
                });

            UpdateText();
            
            if (_vo.IsMax())
                Complete();
            
            return true;
        }

        public override bool RemoveItem(out StackableItemVO removedItem)
        {
            removedItem = null;
            return false;
        }

        public override bool RemoveItems(out List<StackableItemVO> removedItems)
        {
            removedItems = null;
            return false;
        }

        protected virtual void Complete()
        {
            Completed?.Invoke();
        }
    }
}
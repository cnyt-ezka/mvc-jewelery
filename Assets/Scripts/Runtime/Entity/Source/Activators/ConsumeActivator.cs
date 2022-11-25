using System;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Views.Source.AreaBase;
using UnityEngine;
using TMPro;

namespace Runtime.Entity.Source.Activators
{
    public class ConsumeActivator : Activator
    {
        [SerializeField]
        protected TextMeshProUGUI _text;
        private IActivatorTrigger _lastActivatorTrigger;
        private float _durationTime = 0;
        private Tween _imageFillAmountTween;

        public override void Setup(ActivationVO activation, Action onCompleteAction)
        {
            _vo = activation;
            ResetActivator();
            Completed = onCompleteAction;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isProcessing)
                return;

            var activator = other.GetComponentInParent<IActivatorTrigger>();
            if (activator == null)
                return;
            
            _isProcessing = true;
            _lastActivatorTrigger = activator;
        }

        private void OnTriggerStay(Collider other)
        {
            var activator = other.GetComponentInParent<IActivatorTrigger>();
            if (activator == null)
                return;

            if (_lastActivatorTrigger == activator )
                RunProcess();
        }
        private void RunProcess()
        {
            if (_isCompleted)
                return;
            
            _durationTime += Time.deltaTime;
            if (_durationTime >= _vo.Time)
            {
                _isCompleted = true;
                _durationTime = _vo.Time;
            }

            Fill();
        }

        private void OnTriggerExit(Collider other)
        {
            var activator = other.GetComponentInParent<IActivatorTrigger>();
            if (activator == null)
                return;

            if (_lastActivatorTrigger != activator)
                return;

            if (_isCompleted)
                return;

            ResetActivator();
            Fill(.5f);
        }

        private void Fill(float animTime = .1f)
        {
            float startValue = _fillImage.fillAmount;
            float endValue = Mathf.Round(_vo.CurrentAmount / _vo.MaxAmount);
            
            _imageFillAmountTween?.Kill();
            _imageFillAmountTween = DOVirtual.Float(startValue, endValue, animTime, (float updatedValue) =>
            {
                _fillImage.fillAmount = updatedValue;
            }).SetEase(Ease.Linear).OnComplete(TweenCompleted);
        }

        private void TweenCompleted()
        {
            if (!_isCompleted)
                return;

            if (_vo.IsLoop)
            {
                _isCompleted = false;
                _durationTime = 0;
                _imageFillAmountTween?.Kill();
                _fillImage.fillAmount = 0;
            }
            else
            {
                _isProcessing = true;
                _lastActivatorTrigger = null;
                _isCompleted = false;
                _durationTime = 0;
            }
            Completed?.Invoke();
        }

        public override void ResetActivator()
        {
            _imageFillAmountTween?.Kill();
            _isProcessing = false;
            _isCompleted = false;
            _lastActivatorTrigger = null;
            _durationTime = 0;
            _fillImage.fillAmount = 0;
        }

        public override void DestroyActivator()
        {
        }

        public override void SetMax()
        {
            _text.text = "MAX";
            
            if (!_imageFillAmountTween.IsActive())
                _fillImage.fillAmount = 1;
        }
    }
}


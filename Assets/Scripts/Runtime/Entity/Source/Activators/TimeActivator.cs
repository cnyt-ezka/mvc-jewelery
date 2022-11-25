using System;
using DG.Tweening;
using Runtime.Data.ValueObject.Source;
using Runtime.Views.Source.AreaBase;
using UnityEngine;

namespace Runtime.Entity.Source.Activators
{
    public class TimeActivator : Activator
    {
        private IActivatorTrigger _lastActivatorTrigger;
        private Vector3 _lastPos;
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
            _lastPos = other.transform.position;
        }

        private void OnTriggerStay(Collider other)
        {
            var activator = other.GetComponentInParent<IActivatorTrigger>();
            if (activator == null)
                return;
            
            if (_lastActivatorTrigger != activator)
                return;
                    
            if (_lastPos == other.transform.position)
                RunProcess();
            else
                _lastPos = other.transform.position;
        }

        private void OnTriggerExit(Collider other)
        {
            var activator = other.GetComponentInParent<IActivatorTrigger>();
            if(activator == null)
                return;
            
            if (_lastActivatorTrigger != activator)
                return;

            if (_isCompleted)
                return;

            ResetActivator();
            Fill(.5f);
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

        private void Fill(float animTime = .1f)
        {
            float startValue = _fillImage.fillAmount;
            float endValue = (float) _durationTime / _vo.Time;
            
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
                _lastPos = Vector3.zero;
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
            _lastPos = Vector3.zero;
            _durationTime = 0;
            _fillImage.fillAmount = 0;
        }

        public override void DestroyActivator()
        {
        }

        public override void SetMax()
        {
        }
    }
}


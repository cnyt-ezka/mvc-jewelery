using System;
using DG.Tweening;
using MVC.Base.Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Entity.Source.StackableItems
{
    public class StackableItemMotor : MonoBehaviour
    {
        [FoldoutGroup("References")] [SerializeField] private Rigidbody _rigidbody;
        [FoldoutGroup("References")] [SerializeField] private GameObject _colliderParent;
        
        [FoldoutGroup("Collected Motion Settings")] 
        [SerializeField]
        private float _heightOffset = 10f;
        [FoldoutGroup("Collected Motion Settings")]
        [SerializeField]
        private float _minDropForce = 500f;
        [FoldoutGroup("Collected Motion Settings")] 
        [SerializeField]
        private float _maxDropForce = 850f;
        [FoldoutGroup("Collected Motion Settings")] 
        [Range(0.01f, 0.1f)] 
        [SerializeField]
        private float _distanceMultiplier = 0.05f;

        private Transform _targetTransform;
        private Vector3 _dynamicTargetOffset;

        private Vector3 _startPosition;
        private Vector3 _startScale;
        
        private Vector3 _ABLerpPosition;
        private Vector3 _BCLerpPosition;
        private Vector3 _AB_BCLerpPosition;
        private float _interpolateAmount;
        private bool _collectMovement;

        private Action _moveToDynamicTargetCallback;

        public void Setup()
        {
            _startScale = transform.localScale;

            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
            }
        }

        private void OnMoveToDynamicTargetCompleted()
        {
            _collectMovement = false;
            
            transform.SetParent(_targetTransform);
            
            _moveToDynamicTargetCallback?.Invoke();
        }

        private void OnStaticMovementCompleted(Action onCompleteCallback)
        {
            // transform.DOLocalRotate(Vector3.zero, 0.2f);
            
            onCompleteCallback?.Invoke();
        }
        
        private void Update()
        {
            if (_collectMovement)
            {
                var endPosition = _targetTransform.position + _dynamicTargetOffset;
                var peekPosition = ((_startPosition + endPosition) * 0.5f) + Vector3.up * _heightOffset;

                var distance = Vector3.Distance(_startPosition, endPosition);
                
                _interpolateAmount = (_interpolateAmount + Time.deltaTime * 1 / (0.5f + distance * _distanceMultiplier));

                _ABLerpPosition = Vector3.Slerp(_startPosition, peekPosition, _interpolateAmount);
                _BCLerpPosition = Vector3.Slerp(peekPosition, endPosition, _interpolateAmount);
                _AB_BCLerpPosition = Vector3.Slerp(_ABLerpPosition, _BCLerpPosition, _interpolateAmount);

                transform.position = _AB_BCLerpPosition;

                if (_collectMovement)
                {
                    transform.localScale = Vector3.Lerp(_startScale, Vector3.one * _targetSCale, _interpolateAmount);
                    //transform.Rotate(Vector3.one * (300f * Time.deltaTime)); 
                    if (_interpolateAmount >= 1f)
                    {
                        OnMoveToDynamicTargetCompleted();
                    }
                }
            }
        }

        private float _targetSCale;
        public void MoveToDynamicTarget(Transform targetTransform, Vector3 offset, Action onCompleteCallback, float targetScale = 1f)
        {
            Disable();

            _targetSCale = targetScale;
            
            _interpolateAmount = 0;
                
            _targetTransform = targetTransform;
            _dynamicTargetOffset = offset;
            
            _startPosition = transform.position;
            _startScale = transform.localScale;
            
            _collectMovement = true;
            
            _moveToDynamicTargetCallback = onCompleteCallback;
        }

        public void MoveToStaticTarget(Transform targetTransform, Action onCompleteCallback)
        {
            MoveToStaticTarget(targetTransform,targetTransform.position,onCompleteCallback);
        }

        public void MoveToStaticTarget(Transform parent, Vector3 targetPosition, Action onCompleteCallback)
        {
            Disable();
            
            _startPosition = transform.position;
            var endPoint = targetPosition;
            var peekPoint = ((_startPosition + endPoint) * 0.5f).WithY(endPoint.y + _heightOffset * .25f);

            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            
            var path = new[]{_startPosition, peekPoint, endPoint};

            transform.DOScale(Vector3.one, .49f);
            //transform.DOLocalRotate(Vector3.zero, 0.94f);
            transform.DOPath(path, .5f, PathType.CatmullRom)
                .SetEase(Ease.Linear)
                .OnComplete(() => OnStaticMovementCompleted(onCompleteCallback));
        }

        public void DropTo(Vector3 direction, Transform newParent)
        {
            if (_rigidbody == null)
                return;
            
            Enable();

            _colliderParent.SetActive(true);
            
            transform.SetParent(newParent);
            
            _rigidbody.AddForce(direction * Random.Range(_minDropForce, _maxDropForce));
            _rigidbody.AddTorque(Random.insideUnitSphere * Random.Range(25f, 75f));
        }

        public void Enable()
        {
            if (_rigidbody != null)
            {
                _rigidbody.useGravity = true;
                _rigidbody.isKinematic = false;
            }
            
            _colliderParent.SetActive(true);
        }

        public void Disable()
        {
            if (_rigidbody != null)
            {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
            
            _colliderParent.SetActive(false);
        }
    }
}
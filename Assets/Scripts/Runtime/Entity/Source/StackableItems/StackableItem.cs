using System;
using MVC.Base.Runtime.Abstract.Function;
using Runtime.Data.ValueObject.Source;
using UnityEngine;

namespace Runtime.Entity.Source.StackableItems
{
    [RequireComponent(typeof(StackableItemMotor))]
    public class StackableItem : MonoBehaviour, IPoolable
    {
        private StackableItemMotor _motor;

        [SerializeField] private StackableItemVO _vo;
        
        public StackableItemVO VO
        {
            get => _vo; 
            private set => _vo = value; 
        }

        public Action<StackableItem> MoveToDynamicTargetCompleteCallback { get; set; }
        public Action<StackableItem> MoveToStaticTargetCompleteCallback { get; set; }
        
        public void Setup(StackableItemVO vo)
        {
            if (vo.Type != VO.Type)
            {
                Debug.LogError($"VO type is different than prefab on setup {VO.Type}/{vo.Type}");
                return;
            }

            if (vo.Amount != VO.Amount)
            {
                Debug.LogError($"VO amount is different than prefab on setup {VO.Amount}/{vo.Amount}");
                return;   
            }
            
            VO = vo;
            VO.Obj = gameObject;
            
            PoolKey = VO.Type.ToString();

            _motor = GetComponent<StackableItemMotor>();
            _motor.Setup();
            
            SetPosition();
        }

        private void SetPosition()
        {
            transform.localPosition = VO.Position;
            transform.localEulerAngles = VO.Rotation;
        }

        public void Enable()
        {
            _motor.Enable();
        }

        public void Disable()
        {
            _motor.Disable();
        }

        public void DropTo(Vector3 direction, Transform newParent)
        {
            _motor.DropTo(direction, newParent);
        }

        public void MoveToStaticTarget(Transform parent, Vector3 targetPosition, Action<StackableItem> onCompleteCallback)
        {
            _motor.MoveToStaticTarget(parent, targetPosition, () => onCompleteCallback?.Invoke(this));
        }

        public void MoveToStaticTarget(Transform targetTransform, Action<StackableItem> onCompleteCallback)
        {
            _motor.MoveToStaticTarget(targetTransform, () => onCompleteCallback?.Invoke(this));
        }

        public void MoveToDynamicTarget(Transform targetTransform, Vector3 offset, Action<StackableItem> onCompleteCallback, float targetScale = 1f)
        {
            _motor.MoveToDynamicTarget(targetTransform, offset, () => onCompleteCallback?.Invoke(this), targetScale);
        }

        #region Poolable

        public string PoolKey { get; set; }

        public void OnGetFromPool()
        {
            
        }

        public void OnReturnToPool()
        {
            //TODO (GK) 
        }

        #endregion

    }
}
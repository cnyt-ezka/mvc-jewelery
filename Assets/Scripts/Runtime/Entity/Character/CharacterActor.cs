using System;
using Runtime.Data.ValueObject.Character;
using UnityEngine;

namespace Runtime.Entity.Character
{
    public class CharacterActor : MonoBehaviour
    {
        private CharacterVO _vo;

        // [SerializeField]
        // private CharacterHealth _characterHealth;
        [SerializeField]
        private CharacterMotor _motor;
        [SerializeField]
        private CharacterAnimator _animator;

        public Transform Transform => transform;
        
        public void Setup(CharacterVO vo)
        {
            _vo = vo;
            _vo.Obj = gameObject;
            
            //_characterHealth.Setup(vo.Health, OnHealthChanged);
            _motor.Setup(vo.Movement, OnReachedToDestination);
            _animator.Setup();
        }
/*
        #region Health
        
        public int CurrentHealth => _characterHealth.VO.CurrentHealth;
        public int MaxHealth => _characterHealth.VO.MaxHealth;
        public bool IsDead => _characterHealth.VO.IsDead;
        public Action<int, int> OnHealthChanged { get; set; }

        public void AddHealth(int amount)
        {
            _characterHealth.AddHealth(amount);
        }

        public void DecreaseHealth(int amount)
        {
            _characterHealth.DecreaseHealth(amount);
        }

        public void SetHealth(int amount)
        {
            _characterHealth.SetHealth(amount);
        }

        #endregion
*/
        #region Motor
        
        public bool HasReachedToDestination => _motor.HasReachedToDestination;
        public Action OnReachedToDestination { get; set; }

        public float GetCurrentSpeed()
        {
            return _motor.GetCurrentSpeed();
        }

        public float GetMaxSpeed()
        {
            return _motor.GetMaxSpeed();
        }

        public bool IsAgentActive()
        {
            return _motor.IsAgentActive();
        }

        public void EnableAgent()
        {
            _motor.EnableAgent();
        }

        public void DisableAgent()
        {
            _motor.DisableAgent();
        }

        public void SetAgentSpeed(float speed)
        {
            _motor.SetAgentSpeed(speed);
        }

        public void SetDestination(Vector3 destination)
        {
            _motor.SetDestination(destination);
        }

        public void RotateTo(Quaternion lookRotation)
        {
            _motor.RotateTo(lookRotation);
        }

        public void RotateTo(Vector3 direction, bool direct = false)
        {
            _motor.RotateTo(direction, direct);
        }

        public void SetAutoRotation(bool isOn)
        {
            _motor.SetAutoRotation(isOn);
        }

        public void StopMovement()
        {
            _motor.StopMovement();
        }

        #endregion

        #region Animation

        public void SetAnimationTrigger(int hash)
        { 
            _animator.SetAnimationTrigger(hash);
        }

        public void SetAnimationBool(int hash, bool value)
        {
            _animator.SetAnimationBool(hash, value);
        }
        
        public void SetAnimationSpeed(int hash, float speed)
        {
            _animator.SetAnimationSpeed(hash, speed);
        }

        #endregion
    }
}
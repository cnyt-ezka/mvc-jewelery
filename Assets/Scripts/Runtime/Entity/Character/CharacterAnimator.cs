using MVC.Base.Runtime.Extensions;
using UnityEngine;

namespace Runtime.Entity.Character
{
    public interface ICharacterAnimator
    {
        void SetAnimationTrigger(int hash);
        void SetAnimationSpeed(int hash, float speed);
    }
    
    public class CharacterAnimator : MonoBehaviour, ICharacterAnimator
    {
        [SerializeField] 
        private Animator _animator;
        [SerializeField] 
        private CharacterMotor _characterMotor;
        
        public static readonly int INPUT = Animator.StringToHash("Input");

        public void Setup()
        {
            _characterMotor = GetComponent<CharacterMotor>();
        }

        private void Update()
        {
            var animatorInput = _characterMotor.GetCurrentSpeed().Map(0f, _characterMotor.GetMaxSpeed(), 0f, 1f);
            _animator.SetFloat(INPUT, animatorInput);
        }
        
        public void SetAnimationTrigger(int hash)
        {
            _animator.SetTrigger(hash);
        }

        public void SetAnimationSpeed(int hash, float speed)
        {
            _animator.SetFloat(hash, speed);
        }

        public void SetAnimationBool(int hash, bool value)
        {
            _animator.SetBool(hash, value);
        }
    }
}
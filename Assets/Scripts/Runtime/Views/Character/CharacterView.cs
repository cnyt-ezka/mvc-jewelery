using MVC.Base.Runtime.Abstract.View;
using Runtime.Data.ValueObject.Character;
using Runtime.Entity.Character;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Views.Character
{
    public abstract class CharacterView : MVCPoolableView
    {
        #region Installation

        public override bool autoRegisterWithContext => false;

        protected CharacterVO CharacterVO { get; private set; }
        
        protected CharacterActor _characterActor;

        #endregion
        
        public void Setup(CharacterVO vo)
        {
            CharacterVO = vo;
            CharacterVO.Obj = gameObject;
            
            SetPositions();

            _characterActor = GetComponent<CharacterActor>();

            if (_characterActor == null)
            {
                Debug.LogError("Character is not initialized!");
                return;
            }

            _characterActor.OnReachedToDestination += OnReachedToDestination;
            //_characterActor.OnHealthChanged += OnHealthChanged;
            _characterActor.Setup(vo);
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_characterActor == null)
                return;
            
            _characterActor.OnReachedToDestination -= OnReachedToDestination;
            //_characterActor.OnHealthChanged -= OnHealthChanged;
        }

        private void SetPositions()
        {
            transform.position = CharacterVO.Position;
            transform.eulerAngles = CharacterVO.Rotation;
        }

        protected virtual void OnReachedToDestination()
        {
        }

        // private void OnHealthChanged(int currentHealth, int maxHealth)
        // {
        //     
        // }
        
        public void OnJoystickInputReceived(JoystickParam joystickParam)
        {
            // if (_characterActor.IsDead)
            //     return;
            
            _characterActor.SetDestination(_characterActor.Transform.position + joystickParam.DirectionVector3);
        }
        
        public void OnJoystickInputFinished()
        {
            // if (_characterActor.IsDead)
            //     return;
            
            _characterActor.StopMovement(); 
        }
    }
}
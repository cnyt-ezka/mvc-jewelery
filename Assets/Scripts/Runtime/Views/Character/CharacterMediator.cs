using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Character
{
    public class CharacterMediator : Mediator
    {
        protected CharacterView _characterView { get; set; }
        [Inject] protected JoystickSignals _joystickSignals { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();

            _characterView = GetComponent<CharacterView>();
            
            _joystickSignals.Move.AddListener(OnMove);
            _joystickSignals.Stop.AddListener(OnStop);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            
            _joystickSignals.Move.RemoveListener(OnMove);
            _joystickSignals.Stop.RemoveListener(OnStop);

        }


        protected virtual void OnMove(JoystickParam joystickParam)
        {
            _characterView.OnJoystickInputReceived(joystickParam);
        }

        protected virtual void OnStop()
        {
            _characterView.OnJoystickInputFinished();   
        }
    }
}
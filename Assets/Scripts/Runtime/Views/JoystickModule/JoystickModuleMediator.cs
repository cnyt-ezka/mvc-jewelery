using Runtime.Models;
using Runtime.Signals;
using strange.extensions.mediation.impl;

namespace Runtime.Views.JoystickModule
{
    public class JoystickModuleMediator : Mediator
    {
        [Inject] private JoystickModuleView _module { get; set; }
        [Inject] public JoystickSignals _joystick { get; set; }
        
        [Inject] private IGameModel _game { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            _module.JoystickStart+=OnStart;
            _module.JoystickMove+=OnMove;
            _module.JoystickStop+=OnStop;
			
            _joystick.Hide.AddListener(_module.Hide);
            _module.Setup(_game.Status);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            _module.JoystickStart-=OnStart;
            _module.JoystickMove-=OnMove;
            _module.JoystickStop-=OnStop;
        }

        private void OnStart()
        {
            _joystick.Start.Dispatch();
        }

        private void OnMove(JoystickParam joystickParam)
        {
            _joystick.Move.Dispatch(joystickParam);
        }

        private void OnStop()
        {
            _joystick.Stop.Dispatch();
        }
		
    }
}

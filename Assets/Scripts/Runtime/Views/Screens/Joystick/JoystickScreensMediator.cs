using strange.extensions.mediation.impl;

namespace Runtime.Views.Screens.Joystick
{
    public class JoystickScreensMediator : Mediator
    {
        [Inject] public JoystickScreensView view { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }

    }
}
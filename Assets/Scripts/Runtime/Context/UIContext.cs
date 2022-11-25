using MVC.Base.Runtime.Concrete.Context;
using Runtime.Views.JoystickModule;
using Runtime.Views.Screens.Joystick;
using Runtime.Views.Screens.TopBar;

namespace Runtime.Context
{
    public class UIContext : BaseUIContext
    {
        protected override void mapBindings()
        {
            base.mapBindings();

            mediationBinder.Bind<JoystickScreensView>().To<JoystickScreensMediator>();
            mediationBinder.Bind<JoystickModuleView>().To<JoystickModuleMediator>();
            mediationBinder.Bind<TopBarScreensView>().To<TopBarScreensMediator>();
        }

        public override void Launch()
        {
        }
    }
}
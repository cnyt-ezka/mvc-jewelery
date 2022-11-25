using MVC.Base.Runtime.Concrete.Controller;
using MVC.Base.Runtime.Concrete.Injectable.Controller;
using MVC.Base.Runtime.Signals;
using Runtime.Constants;

namespace Runtime.Controller
{
    public class StartGameCommand : MVCCommand
    {
        [Inject] private ScreenSignals _screenSignals { get; set; }
        
        public override void Execute()
        {
            _screenSignals.OpenPanel.Dispatch(new OpenNormalPanelArgs()
            {
                PanelKey = GameScreens.Joystick,
                LayerIndex = 0,
                IgnoreHistory = false
            });
            _screenSignals.OpenPanel.Dispatch(new OpenNormalPanelArgs()
            {
                PanelKey = GameScreens.TopBar,
                LayerIndex = 1,
                IgnoreHistory = false
            });
        }
    }
}
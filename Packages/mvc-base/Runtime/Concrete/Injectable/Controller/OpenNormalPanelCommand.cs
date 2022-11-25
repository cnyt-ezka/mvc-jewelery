using MVC.Base.Runtime.Concrete.Controller;
using MVC.Base.Runtime.Concrete.Data.ValueObject;
using MVC.Base.Runtime.Signals;

namespace MVC.Base.Runtime.Concrete.Injectable.Controller
{
    public class OpenNormalPanelCommand : MVCCommand
    {
        [Inject]
        public ScreenSignals ScreenSignals { get; set; }
        
        [Inject]
        public OpenNormalPanelArgs ParamArgs { get; set; }
        
        public override void Execute()
        {
            PanelVO panelVo = new PanelVO()
            {
                Key = ParamArgs.PanelKey,
                LayerIndex = ParamArgs.LayerIndex,
                Name = ParamArgs.PanelKey,
                IgnoreHistory = ParamArgs.IgnoreHistory,
                Params = ParamArgs.PanelParam
            };
            ScreenSignals.DisplayPanel.Dispatch(panelVo);
        }
    }
}
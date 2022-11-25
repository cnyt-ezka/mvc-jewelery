using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Injectable.Provider;
using MVC.Base.Runtime.Concrete.Controller;
using MVC.Base.Runtime.Concrete.Injectable.Process;

namespace MVC.Base.Runtime.Concrete.Injectable.Controller
{
    public class RetrieveScreenPanelFromResourcesCommand : MVCCommand
    {
        [Inject] public IPanelVO VO {get;set;} //Set by signal
        [Inject] public bool ParamSyncLoad {get;set;} //Set by signal
        [Inject] public IProcessProvider ProcessProvider{get;set;}

        public override void Execute()
        {
            if (!ParamSyncLoad)
            {
                RetrieveResourcesPanelProcess process = ProcessProvider.Get<RetrieveResourcesPanelProcess>();
                process.VO = VO;
                process.AutoReturn = true;
                process.Start();
            }
            else
            {
                RetrieveResourcesPanelSyncProcess panelSyncProcess = ProcessProvider.Get<RetrieveResourcesPanelSyncProcess>();
                panelSyncProcess.PanelVo = VO;
                panelSyncProcess.Start();
            }
        }
    }
}
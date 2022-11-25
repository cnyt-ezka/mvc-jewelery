using MVC.Base.Runtime.Concrete.Controller;

namespace MVC.Base.Runtime.Concrete.Injectable.Controller
{
    public class SROptionsModelRegisterCommand<TModel> : MVCCommand where TModel : class
    {
        [Inject]
        public TModel Model { get; set; }

        public override void Execute()
        {
            if (SRDebug.Instance.Settings.IsEnabled)
                SRDebug.Instance.AddOptionContainer(Model);
        }
    }
}
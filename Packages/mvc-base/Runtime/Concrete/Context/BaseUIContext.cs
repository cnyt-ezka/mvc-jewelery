using MVC.Base.Runtime.Abstract.Injectable.Binder;
using MVC.Base.Runtime.Extensions;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Injectable.Binder;
using MVC.Base.Runtime.Concrete.Injectable.Controller;
using MVC.Base.Runtime.Concrete.Injectable.Mediators;
using MVC.Base.Runtime.Concrete.Model;
using MVC.Base.Runtime.Concrete.Views;
using MVC.Base.Runtime.Signals;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Context
{
    public class BaseUIContext : MVCContext
    {
        protected IMVCScreenBinder screenBinder;
        protected IScreenModel screenModel;
        public BaseUIContext(GameObject root) : base(root)
        {
            Debug.Log("UI Canvas Context Created");
        }

        public BaseUIContext()
        {
            
        }
    
        protected ScreenSignals ScreenSignals;

        protected override void mapBindings()
        {
            base.mapBindings();
            ScreenSignals = injectionBinder.BindCrossContextSingletonSafely<ScreenSignals>();

            injectionBinder.BindCrossContextSingletonSafely<IScreenModel, ScreenModel>();
            
            screenModel = injectionBinder.BindCrossContextSingletonSafely<IScreenModel, ScreenModel>();
            screenBinder = injectionBinder.BindCrossContextSingletonSafely<IMVCScreenBinder, MVCScreenBinder>();
        
            //Binding screen manager.
            mediationBinder.Bind<ScreenManager>().To<ScreenManagerMediator>();
        
            //Commands necessary to open Panels.
            commandBinder.Bind(ScreenSignals.RetrievePanel).To<RetrieveScreenPanelFromResourcesCommand>();
            commandBinder.Bind(ScreenSignals.OpenPanel).To<OpenNormalPanelCommand>();
        }
    
    }
}
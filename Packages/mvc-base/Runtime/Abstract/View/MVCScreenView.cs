using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;

namespace MVC.Base.Runtime.Abstract.View
{
    public class MVCScreenView : MVCView, IPanel
    {
        public virtual string Key => "";
        
        public IPanelVO vo { get; set; }
        
        //We want this to be initialized by ScreenManager
        public override bool autoRegisterWithContext { get=>false; }
    }
    
    public class MVCScreenView<TParam> : MVCView, IPanel
    {
        public virtual string Key => "";
        
        public IPanelVO vo { get; set; }
        
        //We want this to be initialized by ScreenManager
        public override bool autoRegisterWithContext { get=>false; }

        public TParam screenParam;

        private void CastParams()
        {
            screenParam = (TParam) vo.Params;
        }
    }
}
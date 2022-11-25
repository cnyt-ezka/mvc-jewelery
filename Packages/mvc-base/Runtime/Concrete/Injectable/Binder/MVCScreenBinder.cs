using MVC.Base.Runtime.Abstract.Injectable.Binder;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Abstract.View;
using strange.extensions.mediation.api;

namespace MVC.Base.Runtime.Concrete.Injectable.Binder
{
    public class MVCScreenBinder : IMVCScreenBinder
    {
        [Inject] private IMVCMediationBinder _mediationBinder { get; set; }
        [Inject] private IScreenModel _screenModel { get; set; }

        //new IMediationBinding Bind<T> ();
        public IMediationBinding Bind<TScreenView>(string screenKey) where TScreenView : MVCScreenView
        {
            _screenModel.AddMapping(typeof(TScreenView), screenKey);
            return _mediationBinder.Bind<TScreenView>();
        }
        
        public IMediationBinding Bind<TScreenView, TParam>(string screenKey) where TScreenView : MVCScreenView<TParam>
        {
            _screenModel.AddMapping(typeof(TScreenView), screenKey);
            return _mediationBinder.Bind<TScreenView>();
        }
    }
}
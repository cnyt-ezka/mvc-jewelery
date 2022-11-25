using MVC.Base.Runtime.Abstract.View;
using strange.extensions.mediation.api;

namespace MVC.Base.Runtime.Abstract.Injectable.Binder
{
    public interface IMVCScreenBinder
    {
        IMediationBinding Bind<TScreenView>(string screenKey) where TScreenView : MVCScreenView;
        IMediationBinding Bind<TScreenView, TParam>(string screenKey) where TScreenView : MVCScreenView<TParam>;
    }
}
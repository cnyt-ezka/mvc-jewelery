using System.Collections.Generic;
using Runtime.Enums.Source;
using Runtime.Signals;
using strange.extensions.mediation.impl;

namespace Runtime.Views.Source.AreaBase
{
    public class AreaMediator<TView> : Mediator where TView : IArea
    {
        private TView _view;
        
        [Inject] public SourceSignals Signals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            _view = GetComponent<TView>();
            _view.InstallationCompleted += OnInstallationCompleted;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            _view.InstallationCompleted -= OnInstallationCompleted;
        }

        private void OnInstallationCompleted(List<AssetID> result)
        {
            Signals.InstallationCompleted.Dispatch(result);
        }
    }
}

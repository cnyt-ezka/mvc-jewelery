using Runtime.Signals;
using Runtime.Views.Source.AreaBase;

namespace Runtime.Views.Source.Areas.ActivationArea
{
    public class PlayActivationAreaMediator : ActivationAreaMediator
    {
        [Inject] private PlayActivationAreaView _view { get; set; }
        [Inject] private GameSignals _gameSignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            _view.PlayActivated += OnPlayActivated;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            _view.PlayActivated -= OnPlayActivated;
        }

        private void OnPlayActivated()
        {
            //_gameSignals.PlayActivatorActivated.Dispatch();
        }
    }
}

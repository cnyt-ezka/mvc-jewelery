using Runtime.Signals;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class UpgradeCapacityMediator : BaseProcessorAreaMediator
    {
        [Inject] private UpgradeCapacityView _view { get; set; }
        [Inject] private GameSignals _gameSignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            _view.SpeedUpgraded += OnSpeedUpgraded;
        }
        public override void OnRemove()
        {
            base.OnRemove();
            _view.SpeedUpgraded -= OnSpeedUpgraded;
        }

        private void OnSpeedUpgraded()
        {
            _gameSignals.UpgradeCapacityRequest.Dispatch();
        }
    }
}
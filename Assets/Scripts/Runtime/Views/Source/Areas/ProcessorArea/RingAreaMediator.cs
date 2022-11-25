using Runtime.Models;
using Runtime.Signals;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class RingAreaMediator : BaseProcessorAreaMediator
    {
        [Inject] private RingAreaView _view { get; set; }
        [Inject] private IGameModel _game { get; set; }
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private CurrencySignals _currencySignals { get; set; }
        [Inject] private JoystickSignals _jkSignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            _view.BlockStatus += OnBlockStatus;
            _view.UnBlockStatus += OnUnBlockStatus;
            _view.EarnMoney += OnEarnMoney;
        }
        public override void OnRemove()
        {
            base.OnRemove();
            _view.BlockStatus -= OnBlockStatus;
            _view.UnBlockStatus -= OnUnBlockStatus;
            _view.EarnMoney -= OnEarnMoney;
        }

        private void OnEarnMoney(int amount)
        {
            _player.AddCurrency((int)(amount * _player.GetLevel().MainCharacter.GetCurrentIncomeUpgrade().Modifier));
            _currencySignals.CurrencyChanged.Dispatch();

            _game.IsFirstSaleCompleted = true;
            _view.SwitchTimelineAsset();
        }
        private void OnBlockStatus()
        {
            _game.Status.Block();
            _jkSignals.Hide.Dispatch();
        }
        private void OnUnBlockStatus()
        {
            _game.Status.UnBlock();
        }
    }
}
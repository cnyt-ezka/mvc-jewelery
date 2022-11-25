using Runtime.Enums;
using Runtime.Models;
using Runtime.Signals;
using strange.extensions.mediation.impl;

namespace Runtime.Views.Screens.TopBar
{
    public class TopBarScreensMediator : Mediator
    {
        [Inject] private TopBarScreensView _view { get; set; }
        [Inject] private IPlayerModel _playerModel { get; set; }
        [Inject] private CurrencySignals _currencySignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
			
            _currencySignals.CurrencyChanged.AddListener(OnCurrencyChanged);
            
            _view.OnCurrencyChanged(_playerModel.GetCurrency());
        }

        public override void OnRemove()
        {
            base.OnRemove();
			
            _currencySignals.CurrencyChanged.RemoveListener(OnCurrencyChanged);
        }

        private void OnCurrencyReset(GameStatus gameStatus)
        {
            _view.OnCurrencyChanged(0);
        }

        private void OnCurrencyChanged()
        {
            _view.OnCurrencyChanged(_playerModel.GetCurrency());
        }
    }
}

using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Models;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controller.Source
{
    public class UpgradeSpeedCommand : MVCCommand
    {
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private GameSignals _gameSignals { get; set; }
        [Inject] private CurrencySignals _currencySignals { get; set; }
        
        public override void Execute()
        {
            var mainCharacter = _player.GetLevel().MainCharacter;
            var nextSpeedUpgradeVO = mainCharacter.GetNextSpeedUpgrade();

            if (nextSpeedUpgradeVO == null)
            {
                Debug.LogWarning("Speed upgrade is already full.");
                return;
            }
            
            if (_player.RemoveCurrency(nextSpeedUpgradeVO.Price))
            {
                mainCharacter.CurrentSpeedGradeIndex++;
                mainCharacter.Movement.MaxMovementSpeed = mainCharacter.GetCurrentSpeedUpgrade().Modifier;
                mainCharacter.Movement.CurrentMovementSpeed = mainCharacter.Movement.MaxMovementSpeed;
                
                _currencySignals.CurrencyChanged.Dispatch();
                _gameSignals.UpgradeSpeedSuccess.Dispatch();
            }
        }
    }
}
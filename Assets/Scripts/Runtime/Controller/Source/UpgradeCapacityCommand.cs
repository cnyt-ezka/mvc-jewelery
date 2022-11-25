using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Models;
using Runtime.Signals;

namespace Runtime.Controller.Source
{
    public class UpgradeCapacityCommand : MVCCommand
    {
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private GameSignals _gameSignals { get; set; }

        public override void Execute()
        {
            var mainCharacter = _player.GetLevel().MainCharacter;
            
            mainCharacter.Stacker.CurrentGradeIndex++;
            _gameSignals.UpgradeCapacitySuccess.Dispatch();
        }
    }
}
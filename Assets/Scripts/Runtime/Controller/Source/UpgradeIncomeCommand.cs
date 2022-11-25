using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Models;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controller.Source
{
    public class UpgradeIncomeCommand : MVCCommand
    {
        [Inject] private IPlayerModel _player { get; set; }
        
        public override void Execute()
        {
            var mainCharacter = _player.GetLevel().MainCharacter;
            mainCharacter.CurrentIncomeGradeIndex++;
        }
    }
}
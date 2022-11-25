using System.Collections.Generic;
using System.Linq;
using MVC.Base.Runtime.Abstract.Controller;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Data.ValueObject.Source;
using Runtime.Enums.Source;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Controller.Source
{
    public class BuildViewsAfterInstallationCommand : MVCCommand
    {
        [Inject] private IGameModel _game { get; set; }
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private IMVCFunctionBinder _functionBinder { get; set; }
        [Inject] private List<AssetID> _result { get; set; }
        
        public override void Execute()
        {
            Debug.Log("//=> BuildViewsAfterInstallationCommand: ");

            var playerLevel = _player.GetLevel();
            
            foreach (var id in _result)
            {
                var activationArea = playerLevel.ActivationAreas.FirstOrDefault(x => x.ID == id);
                if (activationArea  != null)
                {
                    if (activationArea.IsVisible)
                        continue;
                    
                    activationArea.IsVisible = true;
                    
                    _functionBinder.Run<BuildViewCommand<ActivationAreaVO>, ActivationAreaVO, Transform>(activationArea, _game.SourceContainer);
                
                    continue;
                }

                var processorArea = playerLevel.ProcessorAreas.FirstOrDefault(x => x.ID == id);
                if (processorArea  != null)
                {
                    if (processorArea.IsVisible)
                        continue;
                    
                    processorArea.IsVisible = true;
                    
                    _functionBinder.Run<BuildViewCommand<ProcessorAreaVO>, ProcessorAreaVO, Transform>(processorArea, _game.SourceContainer);
                    
                    continue;
                }
            }
        }
    }
}
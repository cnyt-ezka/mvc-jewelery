using System;
using MVC.Base.Runtime.Abstract.Controller;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Controller.Functions;
using Runtime.Models;
using Runtime.Views.Source.AreaBase;
using UnityEngine;

namespace Runtime.Controller
{
    public class RemoveLevelCommand : MVCCommand
    {
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private IGameModel _game { get; set; }
        [Inject] private IPoolModel _pool { get; set; }
        [Inject] private IMVCFunctionBinder _functionBinder { get; set; }

        private Transform _holder;

        public override void Execute()
        {
            Retain();
            var views = _game.SourceContainer.GetComponentsInChildren<IArea>();
            foreach (var view in views)
            {
                view.AreaDestroy();
            }
            
            _pool.RemoveAll();
            
            _functionBinder.Run<UnloadSceneCommand, string, Action>(_player.GetLevel().EnvironmentScene, UnloadSceneCallback);
        }

        private void UnloadSceneCallback()
        {
            _player.SetNewLevel();
            Release();
        }
    }
}
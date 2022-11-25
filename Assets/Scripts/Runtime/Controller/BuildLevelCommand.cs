using System;
using MVC.Base.Runtime.Abstract.Controller;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Controller.Functions;
using Runtime.Controller.Source;
using Runtime.Data.ValueObject;
using Runtime.Data.ValueObject.Source;
using Runtime.Models;
using Runtime.Signals;
using Runtime.Views.Camera;
using strange.extensions.context.api;
using UnityEngine;
//using ElephantSDK;

namespace Runtime.Controller
{
    public class BuildLevelCommand : MVCCommand
    {
        [Inject] private ILevelModel _levelModel { get; set; }        
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private IGameModel _game { get; set; }
        
        [Inject] private GameSignals _gameSignals { get; set; }
        [Inject] private CameraSignals _cameraSignals { get; set; }
        [Inject] private IMVCFunctionBinder _functionBinder { get; set; }
        [Inject(ContextKeys.CONTEXT_VIEW)] public GameObject Root { get; set; }
        
        private Transform _runnerContainer, _sourceContainer;
        private LevelVO _level;

        public override void Execute()
        {
            Retain();
            if (_player.IsNewLevel)
            {
                _level = _levelModel.GetLevel(_player.SetNextLevelIndex());
                _level = _functionBinder.Run<LevelVO, ConvertRuntimeDataFunction<LevelVO>, LevelVO>(_level);
                
                _player.SaveLevel(_level);
            }
            else
            {
                //_level = Player.GetLevel();
                
                //*****
                _level = _levelModel.GetLevel(_player.GetLevelIndex());
                _level = _functionBinder.Run<LevelVO, ConvertRuntimeDataFunction<LevelVO>, LevelVO>(_level);
                
                _player.SaveLevel(_level);
                //*****
            }

            _functionBinder.Run<LoadSceneCommand, string, Action>(_level.EnvironmentScene, SetupLevel);
            
            //Elephant.LevelStarted(Player.GetLevelIndex());
        }

        private void SetupLevel()
        {
            BuildContainers();
            
            BuildAreas();

            BuildMainCharacter();
            
            AnimateCamera(CameraKey.SourceStart, .25f,CameraKey.Source);
            
            Release();
        }

        private void BuildContainers()
        {
            if (_game.SourceContainer == null)
            {
                _sourceContainer = new GameObject("SourceContainer").transform;
                _sourceContainer.transform.SetParent(Root.transform);
                
                _game.SourceContainer = _sourceContainer;
            }
            else
                _sourceContainer = _game.SourceContainer;

            
            _game.SourceContainerStartPos = _level.SourceContainerStartPos;
        }

        private void BuildAreas()
        {
            for (int ii = 0; ii < _level.ActivationAreas.Count; ii++)
            {
                var area = _level.ActivationAreas[ii];
                
                if (!area.IsVisible) continue;
                
                _functionBinder.Run<BuildViewCommand<ActivationAreaVO>, ActivationAreaVO, Transform>(area, _sourceContainer);
            }

            for (int ii = 0; ii < _level.ProcessorAreas.Count; ii++)
            {
                var area = _level.ProcessorAreas[ii];
                
                if (!area.IsVisible) continue;
                
                _functionBinder.Run<BuildViewCommand<ProcessorAreaVO>, ProcessorAreaVO, Transform>(area, _sourceContainer);
            }
        }

        private void BuildMainCharacter()
        {
            var mainCharacter = _player.GetLevel().MainCharacter;
            _functionBinder.Run<BuildViewCommand<MainCharacterVO>, MainCharacterVO, Transform>(_level.MainCharacter, _sourceContainer);
            
            _cameraSignals.SetTarget.Dispatch(mainCharacter.Obj);
        }
        
        private void AnimateCamera(CameraKey camKey, float nextCamDelay, CameraKey nextCamKey = CameraKey.Source)
        {
            _cameraSignals.ChangeSequential.Dispatch(camKey, nextCamDelay, nextCamKey);
        }
    }
}
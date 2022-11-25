using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Context;
using MVC.Base.Runtime.Concrete.Injectable.Controller;
using MVC.Base.Runtime.Concrete.Model;
using MVC.Base.Runtime.Extensions;
using Runtime.Controller;
using Runtime.Controller.Source;
using Runtime.Models;
using Runtime.Signals;
using Runtime.Views.Camera;
using Runtime.Views.Character;
using Runtime.Views.MainCharacter;
using Runtime.Views.Source.Areas.ProcessorArea;
using UnityEngine;


namespace Runtime.Context
{
    public class GameContext :  MVCContext
    {
        private GameSignals _gameSignals;
        private SourceSignals _sourceSignals;

        protected override void mapBindings()
        {
            base.mapBindings();
            
            _gameSignals = injectionBinder.BindCrossContextSingletonSafely<GameSignals>();
            _sourceSignals = injectionBinder.BindCrossContextSingletonSafely<SourceSignals>();
            injectionBinder.BindCrossContextSingletonSafely<CurrencySignals>();
            injectionBinder.BindCrossContextSingletonSafely<JoystickSignals>();
            injectionBinder.BindCrossContextSingletonSafely<CameraSignals>();

            injectionBinder.BindCrossContextSingletonSafely<IGameModel, GameModel>();
            injectionBinder.BindCrossContextSingletonSafely<IGameModel, GameModel>();
            injectionBinder.BindCrossContextSingletonSafely<SROptionsModel>();

            injectionBinder.BindCrossContextSingletonSafely<IPoolModel, PoolModel>();
            injectionBinder.BindCrossContextSingletonSafely<IGameModel, GameModel>();
            injectionBinder.BindCrossContextSingletonSafely<IPlayerModel, PlayerModel>();
            injectionBinder.BindCrossContextSingletonSafely<ILevelModel, LevelModel>();

            mediationBinder.Bind<CameraManager>().To<CameraMediator>();
            mediationBinder.Bind<CharacterView>().To<CharacterMediator>();
            mediationBinder.Bind<MainCharacterView>().To<MainCharacterMediator>();
            mediationBinder.Bind<StoneSpawnerAreaView>().To<StoneSpawnerAreaMediator>();
            mediationBinder.Bind<HammerAreaView>().To<HammerAreaMediator>();
            mediationBinder.Bind<LaserAreaView>().To<LaserAreaMediator>();
            mediationBinder.Bind<PolishAreaView>().To<PolishAreaMediator>();
            mediationBinder.Bind<RingAreaView>().To<RingAreaMediator>();
            mediationBinder.Bind<NecklaceAreaView>().To<NecklaceAreaMediator>();
            mediationBinder.Bind<UpgradeSpeedView>().To<UpgradeSpeedMediator>();
            mediationBinder.Bind<UpgradeCapacityView>().To<UpgradeCapacityMediator>();
            mediationBinder.Bind<UpgradeIncomeView>().To<UpgradeIncomeMediator>();
            mediationBinder.Bind<GateView>().To<GateMediator>();

            commandBinder.Bind(_gameSignals.StartGame)
                .InSequence()
                .To<SROptionsModelRegisterCommand<SROptionsModel>>()
                .To<InitPoolCommand>()
                .To<StartGameCommand>()
                .To<BuildLevelCommand>();
            
            commandBinder.Bind(_sourceSignals.InstallationCompleted).To<BuildViewsAfterInstallationCommand>();
            commandBinder.Bind(_sourceSignals.TrashProductActivated).To<TrashProductActivatedCommand>();
            commandBinder.Bind(_sourceSignals.TryToConsumeForProcessor).To<TryToConsumeForProcessorCommand>();

            commandBinder.Bind(_gameSignals.UpgradeCapacityRequest).To<UpgradeCapacityCommand>(); 
            commandBinder.Bind(_gameSignals.UpgradeSpeedRequest).To<UpgradeSpeedCommand>();
            commandBinder.Bind(_gameSignals.UpgradeIncomeRequest).To<UpgradeIncomeCommand>();
        }

        public override void Launch()
        {
            Application.targetFrameRate = 60;
            _gameSignals.StartGame.Dispatch();
        }
    }
}

using System;
using MVC.Base.Runtime.Abstract.Model;
using Runtime.Entity.Source.Processors;
using Runtime.Entity.Source.StackableItems;
using Runtime.Models;
using Runtime.Signals;
using Runtime.Views.Character;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.MainCharacter
{
    public class MainCharacterMediator : CharacterMediator
    {
        [Inject] private MainCharacterView _mainCharacterView { get; set; }

        [Inject] protected GameSignals _gameSignals { get; set; }
        [Inject] private SourceSignals _sourceSignals { get; set; }
        [Inject] private CurrencySignals _currencySignals { get; set; }
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private IPoolModel _pool { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
			
            _mainCharacterView.ItemAdded += OnItemAdded;
            _mainCharacterView.ItemRemoved += OnItemRemoved;
            _mainCharacterView.TryToConsumeForProcessor += TryToConsumeForProcessor;
            
            _gameSignals.PreparePlayerForTimelineAnim.AddListener(OnMovePlayer);
            _gameSignals.SendProductToPlayerStack.AddListener(OnSendProductToPlayerStack);
            _gameSignals.UpgradeSpeedSuccess.AddListener(OnUpgradeSpeedSuccess);
            _gameSignals.UpgradeCapacitySuccess.AddListener(OnUpgradeCapacitySuccess);
        }

        public override void OnRemove()
        {
            base.OnRemove();
			
            _mainCharacterView.ItemAdded -= OnItemAdded;
            _mainCharacterView.ItemRemoved -= OnItemRemoved;
            _mainCharacterView.TryToConsumeForProcessor -= TryToConsumeForProcessor;

            _gameSignals.PreparePlayerForTimelineAnim.RemoveListener(OnMovePlayer);
            _gameSignals.SendProductToPlayerStack.RemoveListener(OnSendProductToPlayerStack);
            _gameSignals.UpgradeSpeedSuccess.RemoveListener(OnUpgradeSpeedSuccess);
            _gameSignals.UpgradeCapacitySuccess.RemoveListener(OnUpgradeCapacitySuccess);

        }

        private void OnUpgradeCapacitySuccess()
        {
            _mainCharacterView.UpdateCapacity();
        }

        private void OnUpgradeSpeedSuccess()
        {
            _mainCharacterView.UpdateSpeed();
        }

        private void OnSendProductToPlayerStack(StackableItem item)
        {
            _mainCharacterView.Stacker.AddItem(item);
        }

        private void OnMovePlayer(Transform target, float duration, Action callback)
        {
            _mainCharacterView.MoveToDestination(target, duration, callback);
        }
        
        private void OnMoneyCollected(StackableItem money)
        {
            _player.AddCurrency(money.VO.Amount);
            
            _currencySignals.CurrencyChanged.Dispatch();
            
            _pool.Return(money.VO.Obj);
        }

        private void TryToConsumeForProcessor(Transform stackHolder, IProcessor targetProcessor)
        {
            _sourceSignals.TryToConsumeForProcessor.Dispatch(stackHolder, targetProcessor);
        }
        private void OnItemAdded(StackableItem addedItem)
        {
            
        }

        private void OnItemRemoved(StackableItem removedItem)
        {
            
        }
    }
}

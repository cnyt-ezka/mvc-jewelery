using System;
using Runtime.Entity.Source.StackableItems;
using Runtime.Models;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class PolishAreaMediator : BaseProcessorAreaMediator
    {
        [Inject] private PolishAreaView _view { get; set; }
        [Inject] private IGameModel _game { get; set; }
        [Inject] private GameSignals _gameSignals { get; set; }
        [Inject] private JoystickSignals _jkSignals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            _view.BlockStatus += OnBlockStatus;
            _view.UnBlockStatus += OnUnBlockStatus;
            _view.MovePlayerForTimelineAnim += OnMovePlayer;
            _view.SendProductToPlayerStack += OnSendProductToPlayerStack;
        }
        public override void OnRemove()
        {
            base.OnRemove();
            _view.BlockStatus -= OnBlockStatus;
            _view.UnBlockStatus -= OnUnBlockStatus;
            _view.MovePlayerForTimelineAnim -= OnMovePlayer;
            _view.SendProductToPlayerStack -= OnSendProductToPlayerStack;
        }

        private void OnSendProductToPlayerStack(StackableItem item)
        {
            _gameSignals.SendProductToPlayerStack.Dispatch(item);
        }

        private void OnMovePlayer(Transform player, float duration, Action callback)
        {
            _gameSignals.PreparePlayerForTimelineAnim.Dispatch(player, duration, callback);
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
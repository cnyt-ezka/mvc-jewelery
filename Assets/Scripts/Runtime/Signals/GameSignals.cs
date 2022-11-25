using System;
using Runtime.Entity.Source.StackableItems;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    public class GameSignals
    {
        public Signal StartGame = new ();
        public Signal StartLevel = new ();
        public Signal<Transform, float, Action> PreparePlayerForTimelineAnim = new();
        public Signal<StackableItem> SendProductToPlayerStack = new ();
        
        public Signal UpgradeSpeedRequest = new ();
        public Signal UpgradeCapacityRequest = new ();
        public Signal UpgradeIncomeRequest = new ();
        public Signal UpgradeSpeedSuccess = new ();
        public Signal UpgradeCapacitySuccess = new ();
        public Signal UpgradeIncomeSuccess = new ();
    }
}
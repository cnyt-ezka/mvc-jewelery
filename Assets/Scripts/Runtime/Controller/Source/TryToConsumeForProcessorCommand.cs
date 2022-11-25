using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.Processors;
using Runtime.Entity.Source.StackableItems;
using Runtime.Enums;
using Runtime.Enums.Source;
using Runtime.Models;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controller.Source
{
    public class TryToConsumeForProcessorCommand : MVCCommand
    {
        [Inject] private Transform _stackHolder { get; set; }
        [Inject] private IProcessor _targetProcessor { get; set; }
        
        [Inject] private CurrencySignals _currencySignals { get; set; }
        
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private IPoolModel _pool { get; set; }
        
        public override void Execute()
        {
            switch (_targetProcessor.Type)
            {
                case ProcessorType.Install:
                case ProcessorType.Upgrade:
                    if (_player.RemoveCurrency(1))
                    {
                        var money = _pool.Get(PoolKey.Money.ToString(), _stackHolder).GetComponent<StackableItem>();
                        money.Setup(new StackableItemVO()
                        {
                            IsRuntimeData = true,
                            Type = StackableType.Money,
                            Amount = 1,
                        });
                        money.Disable();
                        
                        _targetProcessor.AddItem(money.VO);
                        
                        _currencySignals.CurrencyChanged.Dispatch();
                    }
                    break;
                default:
                    Debug.LogWarning($"???TryToConsumeForProcessorCommand??? {_targetProcessor.Type}");
                    break;
            }
        }
    }
}
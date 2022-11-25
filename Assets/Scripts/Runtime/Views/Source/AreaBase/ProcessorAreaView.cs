using System;
using System.Collections.Generic;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.Processors.NonStackable;
using Runtime.Entity.Source.Processors.Stackable;
using UnityEngine;

namespace Runtime.Views.Source.AreaBase
{
    public interface IProcessorArea : IArea
    {
    }
    public abstract class ProcessorAreaView : AreaView, IProcessorArea
    {
        #region VO setup

        [HideInInspector][SerializeField] private ProcessorAreaVO _configProcessorArea;
        private ProcessorAreaVO _vo => VO as ProcessorAreaVO;
        public override AreaVO VO
        {
            get => _configProcessorArea;
            set => _configProcessorArea = value as ProcessorAreaVO;
        }

        #endregion

        [SerializeField] protected List<SourceProcessor> _sourceProcessors;
        [SerializeField] protected ProductProcessor _productProcessor;
        [SerializeField] protected ConsumeProcessor _upgradeProcessor;

        private Action _productionCompletedCallback;
        protected Action ProductionCompletedCallback => _productionCompletedCallback;
        protected bool IsProducing { get; set; }

        #region Setup
        protected override void AreaSetup()
        {
            SetupProductProcessor();
            SetupSourceProcessors();
            SetupUpgradeProcessor();
            
            _productionCompletedCallback = ProductionCompleted;
        }
        protected void SetupProductProcessor()
        {
            if (_productProcessor == null)
                return;

            if (_vo.IsProducer)
            {
                _productProcessor.gameObject.SetActive(true);
                _productProcessor.Setup(_vo.GetProductStack(), Pool, OnProductRemoved);
            }
            else
            {
                _productProcessor.gameObject.SetActive(false);
            }
        }
        protected void SetupSourceProcessors()
        {
            if (_sourceProcessors.Count == 0)
                return;

            if (_vo.IsProducer && _vo.NeedSource)
            {
                for (int ii = 0; ii < _sourceProcessors.Count; ii++)
                {
                    _sourceProcessors[ii].gameObject.SetActive(true);
                    _sourceProcessors[ii].Setup(_vo.GetSourceStack(ii), OnSourceAdded);
                }
            }
            else
            {
                for (int ii = 0; ii < _sourceProcessors.Count; ii++)
                    _sourceProcessors[ii].gameObject.SetActive(false);
            }

        }
        private void SetupUpgradeProcessor()
        {
            if (_upgradeProcessor == null)
                return;
            
            if (_vo.IsUpgrader)
            {
                _upgradeProcessor.gameObject.SetActive(true);
                _upgradeProcessor.Setup(_vo.GetUpgradeGrade(), Pool, OnUpgradeCompleted);
            }
            else
            {
                _upgradeProcessor.gameObject.SetActive(false);
            }
        }
        #endregion
        protected override void AreaStart()
        {
            TryToStartProduce();
        }
        protected override void AreaClose()
        {
            IsProducing = false;
            
            base.AreaClose();
        }
        public override void AreaDestroy()
        {
            IsProducing = false;
            
            for (int ii = 0; ii < _sourceProcessors.Count; ii++)
                _sourceProcessors[ii].DestroyProcessor();
            
            if (_productProcessor != null)
                _productProcessor.DestroyProcessor();
            
            if (_upgradeProcessor != null)
                _upgradeProcessor.DestroyProcessor();
                
            base.AreaDestroy();
        }
        
        #region Processors
        private void TryToStartProduce()
        {
            if (!_vo.IsProducer)
                return;
            if (IsProducing)
                return;
            if (_vo.GetProductStack().IsFull())
                return;

            if (_vo.NeedSource)
            {
                foreach (var sourceProcessor in _sourceProcessors)
                {
                    var itemsInSourceProcessor = sourceProcessor.GetItems();
                    var requirementItem = _vo.GetRequirementPerProduct(sourceProcessor.GetItemType());

                    if (itemsInSourceProcessor.Count >= requirementItem.MaxAmount)
                        sourceProcessor.PossibleRemoveItemCount = requirementItem.MaxAmount;
                    else
                        return;
                }

                var consumedItems = new List<StackableItemVO>();
                foreach (var sourceProcessor in _sourceProcessors)
                    if (sourceProcessor.RemoveItems(out consumedItems))
                        ProductionStart(consumedItems);
            }
            else
            {
                ProductionStart(null);
            }
        }
        private void ProductionStart(List<StackableItemVO> consumedItems)
        {
            IsProducing = true;
            
            _productProcessor.OnProcessStarted();
            
            Produce(_productionCompletedCallback, consumedItems);
        }
        protected abstract void Produce(Action productionCompletedCallback, List<StackableItemVO> consumedItems = null);
        private void ProductionCompleted()
        {
            IsProducing = false;
            
            _productProcessor.OnProcessFinished();

            TryToStartProduce();
        }
        protected virtual void OnSourceAdded()
        {
            TryToStartProduce();
        }
        protected virtual void OnProductRemoved()
        {
            TryToStartProduce();
        }
        protected virtual void OnUpgradeCompleted()
        {
            _vo.CurrentGradeIndex++;

            SetupSourceProcessors();
            SetupProductProcessor();
            
            if (_vo.CurrentGradeIndex == _vo.Upgrades.Count)
                _upgradeProcessor.SetMax();
            else
                SetupUpgradeProcessor();
        }
        
        #endregion

    }
}

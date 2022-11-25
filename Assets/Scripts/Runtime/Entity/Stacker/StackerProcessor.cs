using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MVC.Base.Runtime.Extensions;
using Runtime.Data.ValueObject.Source;
using Runtime.Data.ValueObject.Stacker;
using Runtime.Entity.Source.Processors;
using Runtime.Entity.Source.Processors.NonStackable;
using Runtime.Entity.Source.StackableItems;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Entity.Stacker
{
    [Flags]
    public enum StackerStatus
    {
        None = 0,
        
        Blocked = 1 << 0,
        ReadyToRequest = 1 << 1,
        WaitingResponse = 1 << 2,
        Full = 1 << 3
    }
    
    public class StackerProcessor : MonoBehaviour
    {
        [ShowInInspector]
        public StackerVO VO { get; private set; }

        [ShowInInspector]
        public StackerStatus StackerStatus { get; private set; }
        
        [SerializeField] 
        private ColliderTransmitter _colliderTransmitter;
        
        [FoldoutGroup("StackHolder")]
        [SerializeField] private Transform _stackHolder;

        private Action<StackableItem> _onItemAddedCallback;
        private Action<StackableItem> _onItemRemovedCallback;
        private Action<StackableItem> _onMoneyCollected;
        public Action<IProcessor> TryToConsumeForProcessor { get; set; }

        public Transform StackHolder => _stackHolder;
        
        private float _cooldownForInteractProcessors;
        
        public void Setup(StackerVO vo, Action<StackableItem> onItemAddedCallback, Action<StackableItem> onItemRemovedCallback, Action<StackableItem> onMoneyCollectedCallback)
        {
            VO = vo;

            _onItemAddedCallback = onItemAddedCallback;
            _onItemRemovedCallback = onItemRemovedCallback;
            _onMoneyCollected = onMoneyCollectedCallback;
            _colliderTransmitter.OnTriggerStayAction = OnTriggerStayCallback;
            
            CreateAlignPoints();
            
            _cooldownForInteractProcessors = VO.CurrentStack.TimeInterval;
        }

        public void Disable()
        {
            _onItemAddedCallback = null;
            _onItemRemovedCallback = null;
            _onMoneyCollected = null;
            _colliderTransmitter.OnTriggerStayAction = null;
            
            CreateAlignPoints();
        }

        #region Alignment

        
        [DisableInEditorMode] [SerializeField]
        private List<Transform> _alignPoints = new ();
        private void CreateAlignPoints()
        {
            ClearAlignPoints();

            var xCount = (int)VO.CurrentStack.Alignment.GridCount.x;
            var zCount = (int)VO.CurrentStack.Alignment.GridCount.y;
            var yCount = (int)100;

            var startX = xCount==1 ? 0 : (VO.CurrentStack.Alignment.Size.x * -.5f);
            var startZ = yCount==1 ? 0 : (VO.CurrentStack.Alignment.Size.y * -.5f);
            var startY = 0;

            var xOffset = xCount==1 ? 0 : (VO.CurrentStack.Alignment.Size.x / (xCount -1));
            var zOffset = yCount==1 ? 0 : (VO.CurrentStack.Alignment.Size.y / (zCount -1));
            var yOffset = VO.CurrentStack.Alignment.HeightInterval;
            
            var capacity = VO.CurrentStack.MaxCount;
            int count = 1;

            for (int yy = 0; yy < yCount; yy++)
            {
                for (int xx = 0; xx < xCount; xx++)
                {
                    for (int zz = 0; zz < zCount; zz++)
                    {
                        //if (xx>0 && yy>0 && zz>0 && xx<xCount-1 && yy<yCount-1 && zz<zCount-1) continue;
                        
                        if(count == capacity+1)
                            return;

                        var newPoint = new GameObject("StackPoint" + count);
                        count++;
                        
                        newPoint.transform.SetParent(StackHolder);
                        newPoint.transform.localEulerAngles = Vector3.zero;
                        newPoint.transform.localScale = VO.CurrentStack.Alignment.Scale;
                        newPoint.transform.localPosition = new Vector3(
                            (startX + (xx * xOffset)).FixNaN(),
                            (startY + (yy * yOffset)).FixNaN(),
                            (startZ + (zz * zOffset)).FixNaN());

                        _alignPoints.Add(newPoint.transform);
                    }
                }
            }
        }
        private void ClearAlignPoints()
        {
            for (int ii = _alignPoints.Count - 1; ii >= 0; ii--)
                Destroy(_alignPoints[ii].gameObject);

            _alignPoints.Clear();
        }
        private void AlignStack()
        {
            for (int ii = 0; ii < VO.CurrentStack.StackableItems.Count; ii++)
            {
                var item = VO.CurrentStack.StackableItems[ii].Obj.transform;
                item.SetParent(_alignPoints[ii]);
                    
                item.localPosition = Vector3.zero;
                item.localEulerAngles = VO.CurrentStack.Alignment.Rotation;
                item.localScale = VO.CurrentStack.Alignment.Scale;
            }
        }
        private Transform GetLastAlignPoint()
        {
            return _alignPoints[VO.CurrentStack.TempStackCount];
        }
        #endregion
        public void Request(IProcessor processor)
        {
            switch (processor.Type)
            {
                case ProcessorType.Install:
                case ProcessorType.Upgrade:
                    var consumeProcessor = processor.Transform.GetComponent<ConsumeProcessor>();

                    if (consumeProcessor.IsBusy())
                        return;
                    if (consumeProcessor.IsMax())
                        return;

                    TryToConsumeForProcessor?.Invoke(processor);
                    break;

                case ProcessorType.Product:
                    if (VO.IsFull())
                        return;

                    StackableItemVO itemToAdd;
                    if (processor.RemoveItem(out itemToAdd))
                    {
                        var newItem = itemToAdd.Obj.GetComponent<StackableItem>();
                        AddItem(newItem);
                    }
                    break;
                
                default:
                    StackableItemVO removedItemVO = VO.GetStackableItemToExport(processor.GetItemType());
                    if (removedItemVO == null)
                        return;
                    
                    if (processor.AddItem(removedItemVO))
                        OnStackableItemRemoved(removedItemVO.Obj.GetComponent<StackableItem>());
                    
                    break;
            }
        }
        private void OnStackableItemCollected(StackableItem stackableItem)
        {
            VO.Add(stackableItem.VO);
            
            stackableItem.transform.DOLocalRotate(VO.CurrentStack.Alignment.Rotation, 0.2f);
            AlignStack();
            
            _onItemAddedCallback?.Invoke(stackableItem);
        }
        private void OnStackableItemRemoved(StackableItem stackableItem)
        {
            VO.Remove(stackableItem.VO);
            AlignStack();
                    
            VO.CurrentStack.TempStackCount--;
            
            _onItemRemovedCallback?.Invoke(stackableItem);
        }
        private void OnTriggerStayCallback(Collider other)
        {
            if (other.CompareTag("Processor"))
            {
                if (other.transform.parent != null && other.transform.parent.TryGetComponent<IProcessor>(out var processor))
                    if (VO.IsPossibleToInteract(processor.GetItemType()))
                        Request(processor);
            }
            else if (other.CompareTag("StackableItem"))
            {
                if (VO.IsFull())
                    return;
                
                var stackableItem = other.GetComponentInParent<StackableItem>();

                if (VO.IsPossibleToInteract(stackableItem.VO.Type))
                    AddItem(stackableItem);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Processor") )
            {
                _cooldownForInteractProcessors = VO.CurrentStack.TimeInterval;
            }
        }
        public void CollectMoney(StackableItem money)
        {
            money.MoveToDynamicTarget(transform, Vector3.zero,
                OnMoneyCollected);
        }
        private void OnMoneyCollected(StackableItem money)
        {
            _onMoneyCollected?.Invoke(money);
        }
        public void AddItem(StackableItem item)
        {
            var alignPoint = GetLastAlignPoint();
            
            if (VO.CurrentStack.TempStackCount < VO.CurrentStack.MaxCount)
                VO.CurrentStack.TempStackCount++;

            item.transform.SetParent(null);
            item.transform.DOScale(VO.CurrentStack.Alignment.Scale, 0.2f);
            item.MoveToDynamicTarget(alignPoint, Vector3.zero, 
                OnStackableItemCollected);
            item.transform.DOLocalRotate(VO.CurrentStack.Alignment.Rotation, 0.2f);
        }
        public void RemoveItem(StackableItem stackableItem)
        {
            OnStackableItemRemoved(stackableItem);
        }
        public void ShowItems()
        {
            foreach (var stackableItemVO in VO.CurrentStack.StackableItems)
            {
                var view = stackableItemVO.Obj.GetComponent<StackableItem>();
                view.gameObject.SetActive(true);
            }
        }
        public void HideItems()
        {
            foreach (var stackableItemVO in VO.CurrentStack.StackableItems)
            {
                var view = stackableItemVO.Obj.GetComponent<StackableItem>();
                view.gameObject.SetActive(false);
            }
        }
        public void DeleteItems()
        {
            for (int ii = VO.CurrentStack.StackableItems.Count - 1; ii >= 0; ii--)
            {
                var view = VO.CurrentStack.StackableItems[ii].Obj.GetComponent<StackableItem>();
                
                VO.Remove(view.VO);
                Destroy(view);
            }
            
            VO.CurrentStack.TempStackCount = 0;
        }
        public void UpdateCapacity()
        {
            var carryingItems = VO.StackGrades[VO.CurrentGradeIndex-1].StackableItems;//_stackHolder.GetComponentsInChildren<StackableItem>().Where(x=> !x.VO.Type.HasFlag(StackableType.Money)).ToArray();
            foreach (var item in carryingItems)
            {
                var itemTransform = item.Obj.transform;
                itemTransform.SetParent(StackHolder);
            }
            
            CreateAlignPoints();

            for (int ii = 0; ii < carryingItems.Count; ii++)
            {
                carryingItems[ii].Obj.transform.SetParent(_alignPoints[ii]);
                carryingItems[ii].Obj.transform.localScale = VO.CurrentStack.Alignment.Scale;
                carryingItems[ii].Obj.transform.localEulerAngles = VO.CurrentStack.Alignment.Rotation;
                VO.CurrentStack.StackableItems.Add(carryingItems[ii]);
            }

            VO.CurrentStack.TempStackCount = VO.CurrentStack.StackableItems.Count;
            carryingItems.Clear();
        }
    }
}

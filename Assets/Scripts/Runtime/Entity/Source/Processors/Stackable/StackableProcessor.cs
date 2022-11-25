using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Extensions;
using Runtime.Data.ValueObject.Source;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Entity.Source.Processors.Stackable
{
    public abstract class StackableProcessor : MonoBehaviour, IProcessor
    {
        public Transform Transform => transform;
        
        [SerializeField]
        protected ProcessorVO _configVO;
        protected StackVO _vo;
        protected IPoolModel _pool;
        [SerializeField]
        protected Transform _stackHolder;
        public Transform StackHolder => _stackHolder;
        
        [DisableInEditorMode] [SerializeField]
        protected List<Transform> _alignPoints = new ();
        public virtual void Setup(StackVO vo, IPoolModel pool, Action callback)
        {
            Setup(vo, callback);
            
            _pool = pool;
        }
        public virtual void Setup(StackVO vo, Action callback)
        {
            _vo = vo;
            _vo.TempStackCount = vo.StackableItems.Count;
            
            CreateAlignPoints();
        }
        
        public virtual Transform GetLastAlignPoint()
        {
            return _alignPoints[_vo.StackableItems.Count];
        }
        public virtual void AlignStack()
        {
            for (int ii = 0; ii < _vo.StackableItems.Count; ii++)
            {
                var item = _vo.StackableItems[ii].Obj.transform;
                item.SetParent(_alignPoints[ii]);
                    
                item.localPosition = Vector3.zero;
                item.localEulerAngles = Vector3.zero;
                item.localScale = Vector3.one;
            }
        }
        
        public float GetTimeInterval()
        {
            return _vo.TimeInterval;
        }
        public virtual void DestroyProcessor()
        {
            for (int ii = 0; ii < _vo.StackableItems.Count; ii++)
            {
                var item = _vo.StackableItems[ii].Obj;
                _pool.Return(item);
            }
        }
        public abstract void OnProcessStarted();
        public abstract void OnProcessFinished();
        
        #region IProcessor Methods
        public ProcessorType Type => _configVO.Type;
        public StackableType GetItemType()
        {
            return _vo.GetItemType();
        }
        public abstract bool AddItem(StackableItemVO newItem);
        public abstract bool RemoveItem(out StackableItemVO removedItem);
        public abstract bool RemoveItems(out List<StackableItemVO> removedItems);

        #endregion

        #region Alignment
        protected virtual void CreateAlignPoints()
        {
            ClearAlignPoints();

            var xCount = (int)_vo.Alignment.GridCount.x;
            var zCount = (int)_vo.Alignment.GridCount.y;
            var yCount = (int)100;

            var startX = xCount==1 ? 0 : (_vo.Alignment.Size.x * -.5f);
            var startZ = yCount==1 ? 0 : (_vo.Alignment.Size.y * -.5f);
            var startY = 0;

            var xOffset = xCount==1 ? 0 : (_vo.Alignment.Size.x / (xCount -1));
            var zOffset = yCount==1 ? 0 : (_vo.Alignment.Size.y / (zCount -1));
            var yOffset = _vo.Alignment.HeightInterval;
            
            var capacity = _vo.MaxCount;
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
                        newPoint.transform.localScale = _vo.Alignment.Scale;
                        newPoint.transform.localPosition = new Vector3(
                            (startX + (xx * xOffset)).FixNaN(),
                            (startY + (yy * yOffset)).FixNaN(),
                            (startZ + (zz * zOffset)).FixNaN());

                        _alignPoints.Add(newPoint.transform);
                    }
                }
            }
        }
        protected virtual void ClearAlignPoints()
        {
            for (int ii = _alignPoints.Count - 1; ii >= 0; ii--)
                Destroy(_alignPoints[ii].gameObject);

            _alignPoints.Clear();
        }

        #endregion
        
        
    }
}
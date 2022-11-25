using System;
using System.Collections.Generic;
using DG.Tweening;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Abstract.View;
using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.Processors.NonStackable;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Views.Source.AreaBase
{
    public interface IArea
    {
        void SetPositions();
        void AreaDestroy();
        
        Action<List<AssetID>> InstallationCompleted { get; set; }
    }

    public abstract class AreaView : MVCPoolableView, IArea
    {
        [PropertyOrder(-1)][ShowInInspector] public virtual AreaVO VO { get; set; } = new();
        
        public override bool isInjectable => true; //base.

        [Inject] public IPoolModel Pool { get; set; }
        
        [SerializeField] protected InstallationProcessor _installationProcessor;
        
        [SerializeField] protected Transform _areaHolder;

        public Action<List<AssetID>> InstallationCompleted { get; set; }

        public void Setup<T>(T vo) where T : AreaVO
        {
            VO = vo;
            VO.Obj = gameObject;
            SetPositions();
            AreaSetup();

            if (!VO.IsInstalled)
            {
                _installationProcessor.gameObject.SetActive(true);
                _installationProcessor.Setup(VO.InstallationCost, Pool, OnInstallationCompleted);
                _areaHolder.gameObject.SetActive(false);
            }
            else
            {
                if (_installationProcessor != null)
                    _installationProcessor.gameObject.SetActive(false);
                _areaHolder.gameObject.SetActive(true);
                AreaStart();
            }
        }
        public virtual void SetPositions()
        {
            transform.localPosition = VO.Position;
            transform.localEulerAngles = VO.Rotation;
        }

        protected virtual void AreaClose()
        {
            DOVirtual.DelayedCall(.2f, () =>
            {
                _areaHolder.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).OnComplete(AreaDestroy);
            });
        }

        public virtual void AreaDestroy()
        {
            VO.IsInstalled = false;
            VO.IsVisible = false;

            Pool.Return(VO.Obj);
            VO = new AreaVO();
        }

        protected abstract void AreaSetup();

        protected abstract void AreaStart();

        protected virtual void OnInstallationCompleted()
        {
            DOVirtual.DelayedCall(.2f, () =>
            {
                _areaHolder.localScale = Vector3.one * .5f;
                _areaHolder.gameObject.SetActive(true);
                _areaHolder.DOScale(Vector3.one,.5f).SetEase(Ease.OutBack).OnComplete(AreaStart);
            });
            VO.IsInstalled = true;
            InstallationCompleted?.Invoke(VO.InstallationResult);
        }
    }
}

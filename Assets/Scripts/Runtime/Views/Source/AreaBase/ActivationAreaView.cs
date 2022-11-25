using Runtime.Data.ValueObject.Source;
using Runtime.Entity.Source.Activators;
using UnityEngine;

namespace Runtime.Views.Source.AreaBase
{
    public interface IActivatorTrigger
    {
    }
    public interface IActivationArea : IArea
    {
        void ResetActivator();
    }
    public abstract class ActivationAreaView : AreaView, IActivationArea
    {
        #region VO setup

        [HideInInspector][SerializeField] private ActivationAreaVO _configActivationArea;
        private ActivationAreaVO _vo => VO as ActivationAreaVO;
        public override AreaVO VO
        {
            get => _configActivationArea;
            set => _configActivationArea = value as ActivationAreaVO;
        }

        #endregion

        [SerializeField] private TimeActivator TimeActivator;

        protected override void AreaSetup()
        {
            SetupActivator();
        }

        protected override void AreaStart()
        {
            
        }

        private void SetupActivator()
        {
            TimeActivator.Setup(_vo.Activation, OnCompleteAction);
        }
        
        protected abstract void OnCompleteAction();
        
        public void ResetActivator()
        {
            TimeActivator.ResetActivator();
        }

        public override void AreaDestroy()
        {
            ResetActivator();
            base.AreaDestroy();
        }
    }
}

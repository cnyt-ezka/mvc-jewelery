using System;
using MVC.Base.Runtime.Abstract.Model;
using Runtime.Data.ValueObject.Source;

namespace Runtime.Entity.Source.Processors.NonStackable
{
    public class InstallationProcessor : ConsumeProcessor
    {
        public override void Setup(ConsumeVO vo, IPoolModel pool, Action callback)
        {
            base.Setup(vo, pool, callback);
            
            Enable();
        }
        protected override void Complete()
        {
            Disable();
            base.Complete();
        }
        private void Disable()
        {
            gameObject.SetActive(false);
        }
        private void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}
using Runtime.Views.Source.AreaBase;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class BaseProcessorAreaMediator : ProcessorAreaMediator
    {
        protected BaseProcessorAreaView _baseView { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            _baseView = GetComponent<BaseProcessorAreaView>();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }
		
    }
}

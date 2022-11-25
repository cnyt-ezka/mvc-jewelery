namespace Runtime.Views.Source.AreaBase
{
    public class ProcessorAreaMediator : AreaMediator<IProcessorArea>
    {
        private IProcessorArea _view;

        public override void OnRegister()
        {
            base.OnRegister();
            _view = GetComponent<IProcessorArea>();
        }

        public override void OnRemove()
        {
            base.OnRemove();
			
        }
        
		
    }
}

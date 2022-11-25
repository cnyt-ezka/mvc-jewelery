namespace Runtime.Views.Source.AreaBase
{
    public class ActivationAreaMediator : AreaMediator<IActivationArea>
    {
        private IActivationArea _view;

        public override void OnRegister()
        {
            base.OnRegister();
            _view = GetComponent<IActivationArea>();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }
		
    }
}

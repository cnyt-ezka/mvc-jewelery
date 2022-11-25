using Runtime.Views.Source.AreaBase;

namespace Runtime.Views.Source.Areas.ProcessorArea
{
    public class MoneyAreaMediator : ProcessorAreaMediator
    {
        [Inject] private MoneyAreaView _view { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
        }

        public override void OnRemove()
        {
            base.OnRemove();
        }
        
		
    }
}

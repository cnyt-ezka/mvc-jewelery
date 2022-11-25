using System;
using Runtime.Data.ValueObject.Source;
using Runtime.Views.Source.AreaBase;

namespace Runtime.Views.Source.Areas.ActivationArea
{
    public class PlayActivationAreaView : ActivationAreaView
    {
        private ActivationAreaVO _vo => VO as ActivationAreaVO;

        public Action PlayActivated;

        protected override void OnCompleteAction()
        {
            ResetActivator();
            PlayActivated?.Invoke();
        }
    }
}

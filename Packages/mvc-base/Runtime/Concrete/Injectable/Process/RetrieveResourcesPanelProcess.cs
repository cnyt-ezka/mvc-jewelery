using System.Collections;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Injectable.Process;
using MVC.Base.Runtime.Signals;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Injectable.Process
{
    public class RetrieveResourcesPanelProcess : CoroutineProcess<RetrieveResourcesPanelProcess>
    {
        [Inject] public ScreenSignals ScreenSignals { get; set; }

        public IPanelVO VO;
    
        protected override IEnumerator Routine()
        {
            ResourceRequest request = Resources.LoadAsync("Screens/" + VO.Key, typeof(GameObject));
            yield return request;
            if (request.asset == null)
            {
                Debug.LogError("LoadFromResources! Panel not found!! " + VO.Key);
                yield break;
            }
            ScreenSignals.AfterRetrievedPanel.Dispatch(VO,request.asset as GameObject);
        }
    }
}
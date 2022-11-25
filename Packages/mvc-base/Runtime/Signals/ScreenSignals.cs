using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Concrete.Injectable.Controller;
using strange.extensions.signal.impl;
using UnityEngine;

namespace MVC.Base.Runtime.Signals
{
    public class ScreenSignals
    {
        //Future Idea: To support multi ScreenManagers we should also send a key, and screen managers can be assigned to that key and Panel would only open on that specific Canvas.
        //Should be bound to command that is going to configure PanelVo.
        public Signal<OpenNormalPanelArgs> OpenPanel = new Signal<OpenNormalPanelArgs>();
        //Listened by ScreenManagerMediator
        public Signal<IPanelVO> DisplayPanel = new Signal<IPanelVO>();
        //Binded to a command that starts the process of recieving the Panel.
        public Signal<IPanelVO,bool> RetrievePanel = new Signal<IPanelVO,bool>();
        //Listened by ScreenManagerMediator
        public Signal<int> ClearLayerPanel = new Signal<int>();
        //Sent by Process to retriever of the panel. Listened by ScreenManagerMediator, After this, panel should be displayed
        public Signal<IPanelVO,GameObject> AfterRetrievedPanel = new Signal<IPanelVO, GameObject>();
        //Sent after panel is closed
        public Signal<string, int> OnPanelClosed = new Signal<string, int>();
        //Listened by ScreenManagerMediator
        public Signal GoBackScreen = new Signal();
        //Close splash layer
        public Signal CloseSplashLayer = new Signal();
    }
}
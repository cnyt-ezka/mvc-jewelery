using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Abstract.View;
using MVC.Base.Runtime.Concrete.Views;
using MVC.Base.Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

//Currently multi-screen manager is not supported, only have one of this in your app.
namespace MVC.Base.Runtime.Concrete.Injectable.Mediators
{
    public class ScreenManagerMediator : Mediator
    {
        [Inject] public ScreenManager Manager { get; set; }

        [Inject] public IScreenModel ScreenModel { get; set; }

        [Inject] public ScreenSignals ScreenSignals{get;set;}

        /// <summary>
        /// Works after all bindings are completed. 
        /// Useful to attach listeners
        /// After Awake 
        /// Before Start. 
        /// </summary>
        public override void OnRegister()
        {
            Manager.InitLayers();
            
            ScreenSignals.AfterRetrievedPanel.AddListener(CreatePanel);
            ScreenSignals.DisplayPanel.AddListener(OnDisplayPanel);
            ScreenSignals.ClearLayerPanel.AddListener(OnClearLayer);
            ScreenSignals.GoBackScreen.AddListener(OnBack);
            ScreenSignals.CloseSplashLayer.AddListener(OnCloseSplashLayer);
            
            // foreach (Transform layer in Manager.Layers)
            // {
            //     foreach (Transform panelTransform in layer)
            //     {
            //         var panel = panelTransform.GetComponent<IPanel>();
            //         ScreenModel.CurrentPanels.Add(panel);
            //     }
            // }
        
            //Debug.Log(GetType() + " registered with context");
        }

        /// <summary>
        /// Remove the current page. Check the previous page and load it.
        /// </summary>
        private void OnBack()
        {
            if (ScreenModel.History.Count < 2)
                return;

            ScreenModel.History.RemoveAt(ScreenModel.History.Count - 1);
            IPanelVO prePanelVo = ScreenModel.History[ScreenModel.History.Count - 1];
            ScreenModel.History.RemoveAt(ScreenModel.History.Count - 1);

            //Creating signal argument
            ScreenSignals.DisplayPanel.Dispatch(prePanelVo);
        }

        /// <summary>
        /// Receives the display panel request
        /// </summary>
        private void OnDisplayPanel(IPanelVO panelVo)
        {
            if (panelVo.Key == null)
            {
                Debug.LogError("Panel is null");
                return;
            }
            RetrievePanel(panelVo);
        }

        /// <summary>
        /// Checks if the display panel request is valid and raises retrieve signal.
        /// </summary>
        private void RetrievePanel(IPanelVO panelVo)
        {
            if (panelVo.LayerIndex >= Manager.Layers.Length)
            {
                Debug.LogError("There is no layer " + panelVo.LayerIndex);
                return;
            }

            ScreenSignals.RetrievePanel.Dispatch(panelVo,Manager.SyncLoad);
        }

        /// <summary>
        /// Remove the last screen added by name
        /// </summary>
        /// <param name="panelVo"></param>
        private void RemoveFromHistoryByNameFromLast(string name)
        {
            for (int i = ScreenModel.History.Count - 1; i > 0; i--)
            {
                if (name == ScreenModel.History[i].Name)
                {
                    ScreenModel.History.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Create the panel and set the transform of gameobject
        /// </summary>
        /// <param name="vo"> PanelVo which is stored on View objects, if it is a screen </param>
        /// <param name="template"> Prefab to create </param>
        private void CreatePanel(IPanelVO panelVo, GameObject template)
        {
            if (panelVo.RemoveSamePanels)
                RemoveSamePanels(panelVo.Key,panelVo.LayerIndex);

            if (panelVo.RemoveLayer)
                RemoveLayer(panelVo.LayerIndex);

            if (panelVo.RemoveAll)
                RemoveAllPanels();
        
            //This can be a pool!
            GameObject newPanel = Instantiate(template,Manager.Layers[panelVo.LayerIndex].transform);
            IPanel panel = newPanel.GetComponent<IPanel>();
            MVCView view = panel as MVCView;
            if(view == null)
            {
                Debug.LogError("This is not a view!",newPanel);
                return;
            }
            panel.vo = panelVo;
            view.SendMessage("CastParams", SendMessageOptions.DontRequireReceiver);
            view.Initialize();
        
            newPanel.transform.SetParent(Manager.Layers[panelVo.LayerIndex].transform, false);
            newPanel.transform.localScale = Vector3.one;

            if (!panelVo.IgnoreHistory)
                ScreenModel.History.Add(panel.vo);

            ScreenModel.CurrentPanels.Add(panel);
        }

        /// <summary>
        /// Used to prevent having same panels on a layer
        /// </summary>
        private void RemoveSamePanels(string key, int layerIndex)
        {
            var panelList = Manager.Layers[layerIndex].GetComponentsInChildren<IPanel>();
            foreach (var panel in panelList)
            {
                if(panel.vo.Key != key)
                    continue;
                
                RemoveFromHistoryByNameFromLast(key);
                RemovePanel(panel);
            }
        }

        /// <summary>
        /// Clear all the gameobjecs on the given layer
        /// </summary>
        private void OnClearLayer(int layer)
        {
            RemoveLayer(layer);
        }

        /// <summary>
        /// Clear all gameobjects on layer. Called when loading a new screen
        /// </summary>
        private void RemoveLayer(int voLayerIndex)
        {
            foreach (Transform panelTransform in Manager.Layers[voLayerIndex].transform)
            {
                var panel = panelTransform.GetComponent<IPanel>();
                RemovePanel(panel);
            }
        }

        /// <summary>
        /// Clear all panels on all layers
        /// </summary>
        private void RemoveAllPanels()
        {
            foreach (var panel in ScreenModel.CurrentPanels)
            {
                RemovePanel(panel);
            }

            ScreenModel.CurrentPanels.Clear();
        }
        
        /// <summary>
        /// Closing splash layer that has not any IPanelView or related mediator.
        /// </summary>
        private void OnCloseSplashLayer()
        {
            if (Manager.CloseSplashScreen())
                ScreenSignals.CloseSplashLayer.RemoveListener(OnCloseSplashLayer);
        }
        /// <summary>
        /// Works when connected gameobject is destroyed. 
        /// Useful to remove listeners
        /// Before OnDestroy method
        /// </summary>
        public override void OnRemove()
        {
            ScreenSignals.AfterRetrievedPanel.RemoveListener(CreatePanel);
            ScreenSignals.DisplayPanel.RemoveListener(OnDisplayPanel);
            ScreenSignals.ClearLayerPanel.RemoveListener(OnClearLayer);
            ScreenSignals.GoBackScreen.RemoveListener(OnBack);
            ScreenSignals.CloseSplashLayer.RemoveListener(OnCloseSplashLayer);
        }

        private void RemovePanel(IPanel panel)
        {
            ScreenModel.CurrentPanels.Remove(panel);
            Destroy(panel.gameObject);
            ScreenSignals.OnPanelClosed.Dispatch(panel.vo.Key, panel.vo.LayerIndex);
        }
    }
}
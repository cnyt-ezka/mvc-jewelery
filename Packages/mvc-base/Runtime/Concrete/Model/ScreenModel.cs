using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Abstract.View;
using MVC.Base.Runtime.Concrete.Injectable.Controller;
using MVC.Base.Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Model
{
    public class ScreenModel : IScreenModel
    {
        [PostConstruct]
        public void OnPostConstruct()
        {
            _screenMapping = new Dictionary<Type, string>();
            CurrentPanels = new HashSet<IPanel>();
            History = new List<IPanelVO>();
        }

        [Inject] private ScreenSignals _screenSignals { get; set; }
        
        [ShowInInspector]
        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "Name")]
        public List<IPanelVO> History { get; set; }

        [ShowInInspector]
        Dictionary<Type, string> _screenMapping = new Dictionary<Type, string>();
        
        [ShowInInspector] public HashSet<IPanel> CurrentPanels { get; set; }

        public string GetHistoryData()
        {
            string data = string.Empty;
            foreach (IPanelVO panelVo in History)
            {
                data += panelVo.Name + ",";
            }

            return data;
        }

        public bool HasScreenOpened(string key)
        {
            return CurrentPanels.FirstOrDefault(x => x.vo.Key == key) != null;
        }

        public void OpenScreen<TScreenView>(IPanelVO panelVO) where TScreenView : MVCScreenView
        {
            var key = GetScreenKeyWithType(typeof(TScreenView));
            panelVO.Key = key;
            _screenSignals.DisplayPanel.Dispatch(panelVO);
        }

        public void OpenScreen<TScreenView, TParam>(IPanelVO panelVO, TParam panelParam) where TScreenView : MVCScreenView<TParam>
        {
            var key = GetScreenKeyWithType(typeof(TScreenView));
            panelVO.Key = key;
            panelVO.Params = panelParam;
            _screenSignals.DisplayPanel.Dispatch(panelVO);
        }

        public void OpenScreen<TScreenView>(int layerIndex = 0, bool ignoreHistory = false) where TScreenView : MVCScreenView
        {
            var key = GetScreenKeyWithType(typeof(TScreenView));
            var args = new OpenNormalPanelArgs
            {
                IgnoreHistory = ignoreHistory,
                PanelKey = key,
                LayerIndex = layerIndex
            };
            
            _screenSignals.OpenPanel.Dispatch(args);
        }

        public void OpenScreen<TScreenView, TParam>(int layerIndex = 0, bool ignoreHistory = false, TParam screenParam = default) where TScreenView : MVCScreenView<TParam>
        {
            var key = GetScreenKeyWithType(typeof(TScreenView));
            var args = new OpenNormalPanelArgs
            {
                IgnoreHistory = ignoreHistory,
                PanelKey = key,
                LayerIndex = layerIndex,
                PanelParam = screenParam
            };
            
            _screenSignals.OpenPanel.Dispatch(args);
        }

        public TScreenView GetScreen<TScreenView>() where TScreenView : MVCScreenView
        {
            var panel = CurrentPanels.FirstOrDefault(x => x is TScreenView);
            return panel as TScreenView;
        }

        public TScreenView GetScreen<TScreenView, TScreenParam>() where TScreenView : MVCScreenView<TScreenParam>
        {
            var panel = CurrentPanels.FirstOrDefault(x => x is TScreenView);
            return panel as TScreenView;
        }
        
        public void AddMapping(Type screenViewType, string key)
        {
            if(!_screenMapping.ContainsKey(screenViewType))
                _screenMapping.Add(screenViewType, key);
        }

        public string GetScreenKeyWithType(Type screenType)
        {
            if(!_screenMapping.ContainsKey(screenType))
            {
                Debug.LogError("Screen Mapping Not Found! with this key: " + screenType.Name);
                return null;
            }
                    
            var type = _screenMapping[screenType];
            return type;
        }
    }
}
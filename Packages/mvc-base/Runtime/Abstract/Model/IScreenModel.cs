using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Key;
using MVC.Base.Runtime.Abstract.Data.ValueObject;
using MVC.Base.Runtime.Abstract.Function;
using MVC.Base.Runtime.Abstract.View;

namespace MVC.Base.Runtime.Abstract.Model
{
    public interface IScreenModel
    {
        /// <summary>
        /// Panels list on history
        /// </summary>
        List<IPanelVO> History { get; set; }
        
        HashSet<IPanel> CurrentPanels {get; set;}
        bool HasScreenOpened(string key);

        void OpenScreen<TScreenView>(IPanelVO panelVO) where TScreenView : MVCScreenView;
        void OpenScreen<TScreenView, TParam>(IPanelVO panelVO, TParam panelParam)
            where TScreenView : MVCScreenView<TParam>;
        
        void OpenScreen<TScreenView>(int layerIndex = 0, bool ignoreHistory = false)
            where TScreenView : MVCScreenView;
        void OpenScreen<TScreenView, TParam>(int layerIndex = 0, bool ignoreHistory = false, TParam screenParam = default)
            where TScreenView : MVCScreenView<TParam>;

        TScreenView GetScreen<TScreenView>() where TScreenView : MVCScreenView;
        TScreenView GetScreen<TScreenView, TScreenParam>() where TScreenView : MVCScreenView<TScreenParam>;
        
        void AddMapping(Type screenViewType, string key);
        string GetScreenKeyWithType(Type type);
    }
}
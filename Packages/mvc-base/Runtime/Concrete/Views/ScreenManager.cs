using System.Linq;
using MVC.Base.Runtime.Abstract.View;
using MVC.Base.Runtime.Concrete.Screen;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Views
{
    public class ScreenManager : MVCView
    {
        public bool SyncLoad;
        
        /// <summary>
        /// All layers inside on screenmanager gameobject on hierarchy
        /// </summary>
        public ScreenLayer[] Layers;
        public Transform SplashScreen;

        public bool CloseSplashScreen()
        {
            if (SplashScreen == null)
                return false;

            Destroy(SplashScreen.gameObject);
            return true;
        }

        public void InitLayers()
        {
            var layerList = GetComponentsInChildren<ScreenLayer>();
            Layers = layerList.ToArray();
        }
    }
}
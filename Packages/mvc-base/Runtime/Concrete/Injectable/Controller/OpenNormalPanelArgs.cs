namespace MVC.Base.Runtime.Concrete.Injectable.Controller
{
    [System.Serializable]
    public struct OpenNormalPanelArgs
    {
        public OpenNormalPanelArgs(string panelKey ,bool iIgnoreHistory = false, object panelParam = null)
        {
            PanelKey = panelKey;
            LayerIndex = 0;
            IgnoreHistory = iIgnoreHistory;
            PanelParam = panelParam;
        }

        public OpenNormalPanelArgs(string panelKey,int layerIndex ,bool iIgnoreHistory = false, object panelParam = null)
        {
            PanelKey = panelKey;
            LayerIndex = layerIndex;
            IgnoreHistory = iIgnoreHistory;
            PanelParam = panelParam;
        }


        public string PanelKey;
        public int LayerIndex;
        public bool IgnoreHistory;
        public object PanelParam;
    }
}
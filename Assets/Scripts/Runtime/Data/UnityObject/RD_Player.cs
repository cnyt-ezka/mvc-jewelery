using Runtime.Data.ValueObject;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Player", order = 1)]
    public class RD_Player : ScriptableObject
    {
        public PlayerVO VO;

        #if UNITY_EDITOR
        private PreprocessBuildHooker _hooker = new();
        #endif
        
        [Button(ButtonSizes.Gigantic)]
        public void EditorResetData()
        {
            Reset();
        }

        [Button(ButtonSizes.Gigantic)]
        public void Save()
        {
            ES3.Save("PlayerData", VO);
        }

        public void Reset()
        {
            VO = new PlayerVO();
            Save();
        }

        
    }
    
    #if UNITY_EDITOR
    public class PreprocessBuildHooker : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("PreprocessBuildHooker.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);
            Debug.Log("Clearing PlayerData ");
            
            var playerData = Resources.Load<RD_Player>("Data/PlayerData");
            playerData.Reset();
        }
    }
    #endif
}

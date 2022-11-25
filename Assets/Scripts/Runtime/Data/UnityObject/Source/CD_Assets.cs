using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Runtime.Enums;
using Runtime.Enums.Source;
using Runtime.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using MVC.Base.Editor.Concrete.MVCCodeGeneration.Code;
using UnityEditor;
#endif
namespace Runtime.Data.UnityObject.Source
{
    [CreateAssetMenu(menuName = "Config Data/Asset Key Map", order = 5)]
    public class CD_Assets : SafeSerializedScriptableObject
    {
        public Dictionary<AssetID, PoolKey> Map;
        
        #region AreaID creation
#if UNITY_EDITOR
        [BoxGroup("Add AssetID")]
        [SerializeField][FolderPath]
        private string _projectResourcesPath;
        
        [BoxGroup("Add AssetID")]
        [ShowInInspector]
        private string _key;
        
        [BoxGroup("Add AssetID")]
        [Button(ButtonSizes.Large),GUIColor(.2f,.7f,.2f,1f)]
        protected void CreatePoolKey()
        {
            if (string.IsNullOrEmpty(_key))
                return;

            string poolKeyPath = _projectResourcesPath+ "/" + "AssetID.cs";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(poolKeyPath);

            string data;
            if (obj == null)
            {
                Debug.Log("File does not exist");
                return;
            }
            data = LoadFileOnPath(poolKeyPath);
            Debug.Log("Changing on existing file...");
            
            if(data.Contains(_key))
            {
                Debug.Log("Enum already exists");
                return;
            }
            string addition="%Name%,";
            addition+="\r\t\t";
            addition+="ADDPOINT";
            data = data.Replace("//*ADDITION*//",addition);
            data = data.Replace("%Name%", _key);
            data = data.Replace("ADDPOINT","//*ADDITION*//");
            CodeUtilities.SaveFile(data, poolKeyPath);
            Debug.Log("Added PoolKey");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
        private string LoadFileOnPath(string filePath)
        {
            try
            {
                Debug.Log("Loading File = " + filePath);
                string data;
                string path = filePath;
                StreamReader theReader = new StreamReader(path, Encoding.Default);
                using (theReader)
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return string.Empty;
            }
        }
#endif

        #endregion
    }

}

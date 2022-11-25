using System.Collections.Generic;
using Runtime.Data.ValueObject.Joystick;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject.Joystick
{
    [CreateAssetMenu(menuName = "Config Data/Joystick", order = 11)][HideMonoScript]
    public class CD_Joystick : SerializedScriptableObject
    {
        [GUIColor(.9f,.85f,1f)]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)] [HideLabel]
        public Dictionary<JoystickID,JoystickVO> All = new ();

        #region Editor
#if UNITY_EDITOR || DEBUG
        private Texture2D _scriptTexture;

        [OnInspectorInit]
        private void OnInspectorInit()
        {
            string path = "Assets/Scripts/Runtime/Views/JoystickModule/JoystickModule.png";
            _scriptTexture = new Texture2D(64, 64);
            _scriptTexture.LoadImage( System.IO.File.ReadAllBytes( path ) );
        }
        //[GUIColor(219f/255f,208/255f,200/255f)]
        //[GUIColor(221/255f,122/255f,122/255f)]
        [OnInspectorGUI][PropertyOrder(-1)]
        private void DrawLogo()
        {
            if (_scriptTexture == null) return;
            
            GUILayout.BeginHorizontal();//GUI.skin.box);
            
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.richText = true;
            labelStyle.alignment = TextAnchor.MiddleRight;
            GUILayout.Label("<size=20><color=#dd7a7a><b>JOYSTICK</b></color><color=#dbd0c8ff><i>MODULE</i></color></size>",labelStyle,GUILayout.Height(70));
            
            GUIStyle iconStyle = new GUIStyle();
            iconStyle.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label(this._scriptTexture,iconStyle);
            
            GUILayout.EndHorizontal();
        }
#endif
        #endregion
    }

}



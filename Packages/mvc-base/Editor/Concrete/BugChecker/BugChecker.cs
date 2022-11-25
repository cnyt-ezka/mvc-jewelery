using System;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.BugChecker
{
    public class BugChecker : OdinEditorWindow
    {
        [MenuItem("Tools/MVC/Bug Checker")]
        private static void OpenWindow()
        {
            GetWindow<BugChecker>().Show();
        }
        
        [Button(ButtonSizes.Gigantic), GUIColor(.5f,1,.5f)]
        public void CheckMediators()
        {
            string[] allfiles = System.IO.Directory.GetFiles(Application.dataPath + "/Scripts", "*.cs", System.IO.SearchOption.AllDirectories);
            
            var data = new CheckerData()
            {
                ClassName = "Mediator.cs",
                CompareA = ".AddListener(",
                CompareB = ".RemoveListener(",
                WarningText = "Check AddListeners and RemoveListeners"
            };
            Checker(allfiles, data);
            
            data = new CheckerData()
            {
                ClassName = "Mediator.cs",
                CompareA = "+=",
                CompareB = "-=",
                WarningText = "Check Unity action subscriptions"
            };
            Checker(allfiles, data);
        }
        
        [Button(ButtonSizes.Gigantic), GUIColor(.5f,.5f,1f)]
        public void CheckViews()
        {
            string[] allfiles = System.IO.Directory.GetFiles(Application.dataPath + "/Scripts", "*.cs", System.IO.SearchOption.AllDirectories);
            
            var data = new CheckerData()
            {
                ClassName = "View.cs",
                CompareA = " override ",
                CompareB = "base.",
                WarningText = "You forgot to call base method"
            };
            Checker(allfiles, data);

            CheckUnityMethods(allfiles, data);
        }

        private void Checker(string[] allfiles, CheckerData checkerData)
        {
            int checkedfileCount = 0;
            int foundErrorCount = 0;

            Debug.LogWarning("<color=#FFEFD5>Checking for all " + checkerData.ClassName + "\n Comparing '" + checkerData.CompareA + "' count with '" + checkerData.CompareB + "' count</color>");
            foreach (string path in allfiles)
            {
                if (!path.Contains(checkerData.ClassName))
                    continue;

                checkedfileCount++;

                var text = ExcludeScriptFromProject.LoadScript(path);
                int CompareACount = new Regex(Regex.Escape(checkerData.CompareA)).Matches(text).Count;
                int CompareBCount = new Regex(Regex.Escape(checkerData.CompareB)).Matches(text).Count;

                if (CompareACount != CompareBCount)
                {
                    foundErrorCount++;
                    Debug.LogError("! ! ! " + checkerData.WarningText + " in a " + checkerData.ClassName +
                                   " file ! ! !\n<color=#FF6666>PATH ---></color> " + path + "\n");
                    //var newPath = "Assets/" + path.Split(new string[] { "Assets/" }, StringSplitOptions.None)[1].Replace("\\","/");
                    //Debug.Log(newPath);
                    //Debug.Log("<a href=\'"+newPath+"\' line=\'1\'>"+newPath+":1</a>");
                    //Debug.Log("<a href=\"Assets/Scripts/MovablePlatform.cs\" line=\"7\">Assets/Scripts/MovablePlatform.cs:7</a>");
                }
            }

            if(foundErrorCount > 0)
                Debug.LogWarning("<color=#FFDF00>DONE!</color>\n" + CheckS(checkedfileCount, true)+ "cheked.. <color=#FF6666>And you need to fix " +CheckS(foundErrorCount)+ "shown above</color>");
            else
                Debug.LogWarning("<color=#FFDF00>WELLDONE!</color>\n" + CheckS(checkedfileCount, true)+ "cheked.. It`s all clear.");

        }

        private void CheckUnityMethods(string[] allfiles, CheckerData checkerData)
        {

            Debug.LogWarning("<color=#FFEFD5>Checking for all " + checkerData.ClassName + " files for base unity methods</color>");
            foreach (string path in allfiles)
            {
                if (!path.Contains(checkerData.ClassName))
                    continue;
                
                var text = ExcludeScriptFromProject.LoadScript(path);
                
                CheckMethod(checkerData, text, path, "Awake()");

                CheckMethod(checkerData, text, path, "Start()");
                
                CheckMethod(checkerData, text, path, "OnEnable()");
                
                CheckMethod(checkerData, text, path, "OnDisable()");
                
                CheckMethod(checkerData, text, path, "OnDestroy()");
            }
        }

        private static void CheckMethod(CheckerData checkerData, string text, string path, string testString)
        {
            if (text.Contains(testString))
            {
                if (!text.Contains("override void " + testString))
                {
                    Debug.LogError("! ! ! Please use '<color=#ADFF2F>protected override void " + testString + "</color>' on a " +
                                   checkerData.ClassName +
                                   " file ! ! !\n<color=#FF6666>PATH ---></color> " + path + "\n");
                }
                else if (!text.Contains("base." + testString))
                {
                    Debug.LogError("! ! ! Please use '<color=#ADFF2F>base." + testString + "</color>' on a " + checkerData.ClassName +
                                   " file ! ! !\n<color=#FF6666>PATH ---></color> " + path + "\n");
                }
            }
        }

        public string CheckS(int count, bool hasAre = false)
        {
            if(count>1)
                return count.ToString() + " files "+ (hasAre ? "are ":"");
            else
                return count.ToString() + " file "+ (hasAre ? "is ":"");
        }
    }

    
    public struct CheckerData
    {
        public string CompareA;
        public string CompareB;
        public string ClassName;
        public string WarningText;
    }
}

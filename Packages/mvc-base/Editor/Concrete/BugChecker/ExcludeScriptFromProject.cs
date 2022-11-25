using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.BugChecker
{
    public class ExcludeScriptFromProject : UnityEditor.Editor
    {
        private static Encoding _encoding;

        [MenuItem("Tools/Exclude Script From Project")] // % – CTRL | # – Shift | & – Alt | _ - for single
        [UsedImplicitly]
        public static void ExcludeScriptFromProjectOperation()
        {
            foreach (var item in Selection.objects)
            {
                if (!(item is TextAsset)) continue;

                var script = item as TextAsset;
                var path = AssetDatabase.GetAssetPath(script);
                var text = LoadScript(path);
                if (!text.StartsWith("#if UNITY_EDITOR"))
                {
                    text = "#if UNITY_EDITOR \r\n" + text + "\r\n#endif";
                    text = Regex.Replace(text, @"\r\n|\n\r|\n|\r", "\r\n");
                    SaveFile(text, path);
                }
            }
        }

        public static void SaveFile(string data, string filename)
        {
            using (var outfile = new StreamWriter(filename, false, _encoding))
            {
                outfile.Write(data);
            }
            AssetDatabase.Refresh();
        }

        public static string LoadScript(string path)
        {
            // Debug.Log(path);
            _encoding = GetEncoding(path);
            try
            {
                string data;
                using (var theReader = new StreamReader(path, _encoding))
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}\n", e.Message);
                return string.Empty;
            }
        }

        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }
    }
}
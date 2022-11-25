using Sirenix.OdinInspector;
using UnityEditor;

namespace Runtime.Extensions
{
    public class SafeSerializedScriptableObject : SerializedScriptableObject
    {
#if UNITY_EDITOR
#pragma warning disable CS0414
        private bool _requireValidation = false;
#pragma warning restore CS0414

        private void OnValidate()
        {
            _requireValidation = true;
        }

        [ShowIf(nameof(_requireValidation))]
        [GUIColor(1f, .4f, .4f, 1)]
        [PropertyOrder(100)][Button(100)]
        [PropertySpace(SpaceBefore = 20)]
        private void SaveProject()
        {
            _requireValidation = false;
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }
#endif
    }
}

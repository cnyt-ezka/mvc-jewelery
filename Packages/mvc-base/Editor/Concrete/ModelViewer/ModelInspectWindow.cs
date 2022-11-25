using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Base.Runtime.Concrete.Root;
using Sirenix.OdinInspector.Editor;
using strange.extensions.context.impl;
using strange.extensions.injector.impl;
using strange.framework.api;
using UnityEditor;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.ModelViewer
{
    public class ModelInspectWindow : OdinEditorWindow
    {
        private string _inspectedRootTypeName;
        private string _inspectedObjectTypeName;

        private object _inspectedObject;

        protected override void OnGUI()
        {
            base.OnGUI();
            
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("Enter Play Mode to view data.");
                return;
            }

            if (_inspectedObject == null)
                GetInspectedObject();
        }

        public void SetInspectObject(CrossContext root, object targetObject)
        {
            _inspectedRootTypeName = root.GetType().ToString();
            _inspectedObjectTypeName = targetObject.GetType().ToString();
            
            _inspectedObject = targetObject;
            
            Debug.Log(_inspectedRootTypeName);
        }
        
        private void GetInspectedObject()
        {
            var rootObjects = FindObjectsOfType<MVCContextRoot>();
            var root = rootObjects.FirstOrDefault(x => x.context.GetType().ToString() == _inspectedRootTypeName);
            CrossContext context = root.context as CrossContext;
            if(context == null) return;
        
            FieldInfo fieldInfo = typeof(CrossContextInjectionBinder).BaseType.GetField("bindings",BindingFlags.Instance| BindingFlags.NonPublic);
            
            if(fieldInfo == null) return;
        
            InjectionBinder injectionBinder = context.injectionBinder as CrossContextInjectionBinder;
            
            if(injectionBinder == null) return;
            
            object bindings = fieldInfo.GetValue(injectionBinder);
            Dictionary<object,Dictionary<object,IBinding>> bindingDictionaries = (Dictionary<object,Dictionary<object,IBinding>>)bindings;
            if(bindingDictionaries == null) return;

            foreach (object mainKey in bindingDictionaries.Keys)
            {
                if (!(mainKey is Type))
                {
                    continue;
                }
            
                Type mainKeyType = mainKey as Type;
               
                IBinding binding = injectionBinder.GetBinding(mainKeyType);

                if (binding == null)
                {
                    continue;
                }

                object instance = injectionBinder.GetInstance(mainKeyType);
                if (instance.GetType().ToString() == _inspectedObjectTypeName)
                {
                    _inspectedObject = instance;
                    SetInspectObject(root.context as CrossContext, instance);
                    InspectObject(this, instance);
                }
            }
        }
    }
}
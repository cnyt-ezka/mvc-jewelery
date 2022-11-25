using System;
using System.Collections;
using System.Globalization;
using MVC.Base.Runtime.Abstract.Injectable.Provider;
using MVC.Base.Runtime.Concrete.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Controller.Functions
{
    public class UnloadSceneCommand : MVCVoidFunction<string, Action>
    {
        [Inject] private IUpdateProvider _updateProvider { get; set; }
        
        private Action _callback;

        public override void Execute(string sceneName, Action callback)
        {
            _callback = callback;
            _updateProvider.StartCoroutine(UnloadScene(sceneName));
        }

        private IEnumerator UnloadScene(string name)
        {
            var operation = SceneManager.UnloadSceneAsync(name);
            
            while (!operation.isDone)
            {
                Debug.Log($"Scene {name} Unload Progress {operation.progress.ToString(CultureInfo.InvariantCulture)}");
                yield return null;
            }

            Debug.Log($"Scene {name} Unloaded!");

            _callback?.Invoke();
        }
    }
}
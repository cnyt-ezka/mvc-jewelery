using System;
using Runtime.Data.ValueObject.Source;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace Runtime.Entity.Source.Activators
{
    public abstract class Activator : MonoBehaviour
    {
        public Action Completed { get; set; }

        protected ActivationVO _vo;
        protected bool _isProcessing;
        protected bool _isCompleted;
        [SerializeField] protected ProceduralImage _fillImage;
        public abstract void Setup(ActivationVO activation, Action onCompleteAction);
        public abstract void ResetActivator();
        public abstract void DestroyActivator();
        public abstract void SetMax(); 
    }
}


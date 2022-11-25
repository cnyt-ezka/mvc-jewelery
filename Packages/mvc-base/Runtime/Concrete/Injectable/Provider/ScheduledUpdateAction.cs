using System;
using MVC.Base.Runtime.Abstract.Injectable.Provider;
using UnityEngine;
using UnityEngine.Events;

namespace MVC.Base.Runtime.Concrete.Injectable.Provider
{
    public class ScheduledUpdateAction : IScheduledUpdateAction
    {
        public bool IsRunning { get; private set; }
        public bool HasCompleted => ProgressTime >= RemainingTime;
        
        public float RemainingTime { get; protected set; }
        public float ProgressTime { get; protected set; }

        protected UnityAction _onUpdate;
        protected Action _onComplete;

        public ScheduledUpdateAction(float remainingTime, UnityAction onUpdate, Action onComplete = null)
        {
            RemainingTime = remainingTime;
            _onUpdate = onUpdate;
            _onComplete = onComplete;
        }
        
        public void Run()
        {
            IsRunning = true;
        }
        
        public void AddOnComplete(Action onComplete)
        {
            _onComplete += onComplete;
        }
        
        public void UpdateRemainingTime(float remainingTime)
        {
            ProgressTime = 0;
            RemainingTime = remainingTime;
        }

        public void AddProgressTime(float seconds)
        {
            ProgressTime += seconds;
        }

        public virtual void Tick()
        {
            if(!IsRunning)
                return;
            
            ProgressTime += Time.deltaTime;

            _onUpdate?.Invoke();
            
            if(HasCompleted)
            {
                _onComplete?.Invoke();
                ResetAction();
            }
        }

        public void ResetAction()
        {
            IsRunning = false;
            ProgressTime = 0;
            RemainingTime = 0;
            
            _onComplete = null;
            _onUpdate = null;
        }
    }
}
using System;

namespace MVC.Base.Runtime.Abstract.Injectable.Provider
{
    public interface IScheduledUpdateAction
    {
        bool IsRunning { get; }
        bool HasCompleted { get; }
        float RemainingTime{ get;}
        float ProgressTime{ get;}
        void AddOnComplete(Action onComplete);
        void AddProgressTime(float seconds);
        void UpdateRemainingTime(float remainingTime);
        void Tick();
        void ResetAction();
    }
}
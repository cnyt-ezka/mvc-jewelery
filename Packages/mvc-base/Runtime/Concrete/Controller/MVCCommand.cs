using System;
using MVC.Base.Runtime.Abstract.Controller;
using MVC.Base.Runtime.Extensions.Pool;
using strange.extensions.injector.api;

namespace MVC.Base.Runtime.Concrete.Controller
{
    #region MVCCommand

    public abstract class MVCCommand : IMVCCommand, IMVCPoolableCommand
    {
        [Inject] protected IMVCCommandBinder commandBinder { get; set; }
        [Inject] protected IInjectionBinder injectionBinder { get; set; }

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCCommand()
        {
            IsClean = true;
        }
        
        public Type[] GetGenericArguments()
        {
            return null;
        }
        
        public abstract void Execute();
        
        public void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.Jump<TMVCCommand>(this, sequenceData);
        }

        public void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.JumpBack<TMVCCommand>(this, sequenceData);
        }

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            commandBinder?.ReleaseCommand(this, sequenceData);
        }

        public virtual void Restore()
        {
            injectionBinder.injector.Uninject (this);
            IsClean = true;
        }

        public virtual void Retain()
        {
            retain = true;
        }

        public virtual void Fail()
        {
            commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion

    #region MVCCommand<T>

    public abstract class MVCCommand<TExecute> : IMVCCommand<TExecute>, IMVCPoolableCommand
    {
        public bool IsGenericCommand => true;
        [Inject] protected IMVCCommandBinder commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCCommand()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TExecute)
            };
        }
        
        public abstract void Execute(TExecute executeParameter);

        public void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.Jump<TMVCCommand>(this, sequenceData);
        }
        
        public void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.JumpBack<TMVCCommand>(this, sequenceData);
        }
        
        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            commandBinder?.ReleaseCommand(this, sequenceData);
        }

        public virtual void Restore()
        {
            _injectionBinder.injector.Uninject (this);
            IsClean = true;
        }

        public virtual void Retain()
        {
            retain = true;
        }

        public virtual void Fail()
        {
            commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
    
    #region MVCCommand<T1,T2>

    public abstract class MVCCommand<TExecute1, TExecute2> : IMVCCommand<TExecute1, TExecute2>, IMVCPoolableCommand
    {
        public bool IsGenericCommand => true;
        [Inject] protected IMVCCommandBinder commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCCommand()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TExecute1),
                typeof(TExecute2)
            };
        }
        
        public abstract void Execute(TExecute1 executeParam1, TExecute2 executeParam2);
        
        public void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.Jump<TMVCCommand>(this, sequenceData);
        }

        public void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.JumpBack<TMVCCommand>(this, sequenceData);
        }
        
        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            commandBinder?.ReleaseCommand(this, sequenceData);
        }

        public virtual void Restore()
        {
            _injectionBinder.injector.Uninject (this);
            IsClean = true;
        }

        public virtual void Retain()
        {
            retain = true;
        }

        public virtual void Fail()
        {
            commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
    
    #region MVCCommand<T1,T2,T3>

    public abstract class MVCCommand<TExecute1, TExecute2, TExecute3> : IMVCCommand<TExecute1, TExecute2, TExecute3>, IMVCPoolableCommand
    {
        public bool IsGenericCommand => true;
        [Inject] protected IMVCCommandBinder commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCCommand()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TExecute1),
                typeof(TExecute2),
                typeof(TExecute3)
            };
        }
        
        public abstract void Execute(TExecute1 executeParam1, TExecute2 executeParam2, TExecute3 executeParam3);
        
        public void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.Jump<TMVCCommand>(this, sequenceData);
        }

        public void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.JumpBack<TMVCCommand>(this, sequenceData);
        }
        
        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            commandBinder?.ReleaseCommand(this, sequenceData);
        }

        public virtual void Restore()
        {
            _injectionBinder.injector.Uninject (this);
            IsClean = true;
        }

        public virtual void Retain()
        {
            retain = true;
        }

        public virtual void Fail()
        {
            commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
    
    #region MVCCommand<T1,T2,T3,T4>

    public abstract class MVCCommand<TExecute1, TExecute2, TExecute3, TExecute4> : IMVCCommand<TExecute1, TExecute2, TExecute3, TExecute4>, IMVCPoolableCommand
    {
        public bool IsGenericCommand => true;
        [Inject] protected IMVCCommandBinder commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCCommand()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TExecute1),
                typeof(TExecute2),
                typeof(TExecute3),
                typeof(TExecute4)
            };
        }
        
        public abstract void Execute(TExecute1 executeParam1, TExecute2 executeParam2, TExecute3 executeParam3, TExecute4 executeParam4);
        
        public void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.Jump<TMVCCommand>(this, sequenceData);
        }

        public void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody
        {
            retain = false;
            commandBinder?.JumpBack<TMVCCommand>(this, sequenceData);
        }
        
        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            commandBinder?.ReleaseCommand(this, sequenceData);
        }

        public virtual void Restore()
        {
            _injectionBinder.injector.Uninject (this);
            IsClean = true;
        }

        public virtual void Retain()
        {
            retain = true;
        }

        public virtual void Fail()
        {
            commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
}
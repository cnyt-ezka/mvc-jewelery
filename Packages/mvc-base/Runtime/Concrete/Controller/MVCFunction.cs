using System;
using MVC.Base.Runtime.Abstract.Controller;
using strange.extensions.injector.api;

namespace MVC.Base.Runtime.Concrete.Controller
{
    #region MVCReturnableFunction<TReturn>

    public abstract class MVCFunction<TReturnType> : IMVCFunction<TReturnType>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TReturnType),
            };
        }
        
        public abstract TReturnType Execute();

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion

    #region MVCReturnableFunction<TReturn, TParam>

    public abstract class MVCFunction<TReturnType, TParam> : IMVCFunction<TReturnType, TParam>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TReturnType),
                typeof(TParam)
            };
        }
        
        public abstract TReturnType Execute(TParam param);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion

    #region MVCReturnableFunction<TReturn, TParam1, TParam2>

    public abstract class MVCFunction<TReturnType, TParam1, TParam2> : IMVCFunction<TReturnType, TParam1, TParam2>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TReturnType),
                typeof(TParam1),
                typeof(TParam2)
            };
        }
        
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
    
    #region MVCReturnableFunction<TReturn, TParam1, TParam2, TParam3>
    
    public abstract class MVCFunction<TReturnType, TParam1, TParam2, TParam3> : IMVCFunction<TReturnType, TParam1, TParam2, TParam3>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }

        public bool IsGenericCommand => false;
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCFunction()
        {
            IsClean = true;
        }
        
        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TReturnType),
                typeof(TParam1),
                typeof(TParam2),
                typeof(TParam3)
            };
        }
        
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }
    
    #endregion
    
    #region MVCReturnableFunction<TReturn, TParam1, TParam2, TParam3, TParam4>
    
    public abstract class MVCFunction<TReturnType, TParam1, TParam2, TParam3, TParam4> : IMVCFunction<TReturnType, TParam1, TParam2, TParam3, TParam4>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }

        public bool IsGenericCommand => false;
        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCFunction()
        {
            IsClean = true;
        }
        
        public Type[] GetGenericArguments()
        {
            return new[]
            {
                typeof(TReturnType),
                typeof(TParam1),
                typeof(TParam2),
                typeof(TParam3),
                typeof(TParam4)
            };
        }
        
        public abstract TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }
    
    #endregion

    #region MVCVoidFunction

    public abstract class MVCVoidFunction : IMVCVoidFunction
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCVoidFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return null;
        }
        
        public abstract void Execute();

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion

    #region MVCVoidFunction<TParam>

    public abstract class MVCVoidFunction<TParam> : IMVCVoidFunction<TParam>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCVoidFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new []
            {
                typeof(TParam)
            };
        }
        
        public abstract void Execute(TParam param);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion

    #region MVCVoidFunction<TParam1, TParam2>

    public abstract class MVCVoidFunction<TParam1, TParam2> : IMVCVoidFunction<TParam1, TParam2>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCVoidFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new []
            {
                typeof(TParam1),
                typeof(TParam2)
            };
        }
        
        public abstract void Execute(TParam1 executeParam1, TParam2 executeParam2);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion

    #region MVCVoidFunction<TParam1, TParam2, TParam3>

    public abstract class MVCVoidFunction<TParam1, TParam2, TParam3> : IMVCVoidFunction<TParam1, TParam2, TParam3>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCVoidFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new []
            {
                typeof(TParam1),
                typeof(TParam2),
                typeof(TParam3)
            };
        }
        
        public abstract void Execute(TParam1 executeParam1, TParam2 executeParam2, TParam3 executeParam3);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
    
    #region MVCVoidFunction<TParam1, TParam2, TParam3>

    public abstract class MVCVoidFunction<TParam1, TParam2, TParam3, TParam4> : IMVCVoidFunction<TParam1, TParam2, TParam3, TParam4>
    {
        [Inject] protected IMVCCommandBinder _commandBinder { get; set; }
        [Inject] protected IInjectionBinder _injectionBinder { get; set; }
        
        public bool IsGenericCommand => false;

        public bool IsClean { get; set; }
        public bool retain { get; protected set; }
        public bool cancelled { get; set; }
        public object data { get; set; }
        public int sequenceId { get; set; }

        public MVCVoidFunction()
        {
            IsClean = true;
        }

        public Type[] GetGenericArguments()
        {
            return new []
            {
                typeof(TParam1),
                typeof(TParam2),
                typeof(TParam3),
                typeof(TParam4)
            };
        }
        
        public abstract void Execute(TParam1 executeParam1, TParam2 executeParam2, TParam3 executeParam3, TParam4 executeParam4);

        public virtual void Release(params object[] sequenceData)
        {
            retain = false;
            // _commandBinder?.ReleaseCommand(this, sequenceData);
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
            _commandBinder?.Stop (this);
        }

        public virtual void Cancel()
        {
        }
    }

    #endregion
}
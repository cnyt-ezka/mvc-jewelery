using MVC.Base.Runtime.Abstract.Controller;
using strange.extensions.injector.api;

namespace MVC.Base.Runtime.Concrete.Controller
{
    public class MVCFunctionBinder : IMVCFunctionBinder
    {
        [Inject] public IInjectionBinder injectionBinder { get; set; }
        
        public TReturn Run<TReturn,TCommand>() where TCommand : IMVCFunction<TReturn>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            var result = command.Execute();
            return result;
        }
        
        public TReturn Run<TReturn,TCommand, TParam1>(TParam1 param) where TCommand : IMVCFunction<TReturn, TParam1>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            var result = command.Execute(param);
            return result;
        }

        public TReturn Run<TReturn, TCommand, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TCommand : IMVCFunction<TReturn, TParam1, TParam2>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            var result = command.Execute(param1, param2);
            return result;
        }

        public TReturn Run<TReturn, TCommand, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, 
            TParam3 param3) where TCommand : IMVCFunction<TReturn, TParam1, TParam2, TParam3>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            var result = command.Execute(param1, param2, param3);
            return result;
        }

        public TReturn Run<TReturn, TCommand, TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2,
            TParam3 param3, TParam4 param4) where TCommand : IMVCFunction<TReturn, TParam1, TParam2, TParam3, TParam4>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            var result = command.Execute(param1, param2, param3, param4);
            return result;
        }

        public void Run<TCommand>() where TCommand : IMVCVoidFunction, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            command.Execute();
        }
        
        public void Run<TCommand, TParam1>(TParam1 param1) where TCommand : IMVCVoidFunction<TParam1>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            command.Execute(param1);
        }
        
        public void Run<TCommand, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TCommand : IMVCVoidFunction<TParam1, TParam2>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            command.Execute(param1, param2);
        }
        
        public void Run<TCommand, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3) where TCommand : IMVCVoidFunction<TParam1, TParam2, TParam3>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            command.Execute(param1, param2, param3);
        }
        
        public void Run<TCommand, TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4) where TCommand : IMVCVoidFunction<TParam1, TParam2, TParam3, TParam4>, new()
        {
            var command = new TCommand();
            injectionBinder.injector.Inject(command, true);
            command.Execute(param1, param2, param3, param4);
        }
    }
}
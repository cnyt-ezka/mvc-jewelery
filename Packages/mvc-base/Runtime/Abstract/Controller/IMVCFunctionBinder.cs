namespace MVC.Base.Runtime.Abstract.Controller
{
    public interface IMVCFunctionBinder
    {
        TReturn Run<TReturn, TCommand>() where TCommand : IMVCFunction<TReturn>, new();
        TReturn Run<TReturn, TCommand, TParam1>(TParam1 param) where TCommand : IMVCFunction<TReturn, TParam1>, new();
        TReturn Run<TReturn, TCommand, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TCommand : IMVCFunction<TReturn, TParam1, TParam2>, new();
        TReturn Run<TReturn, TCommand, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3) where TCommand : IMVCFunction<TReturn, TParam1, TParam2, TParam3>, new();
        TReturn Run<TReturn, TCommand, TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4) where TCommand : IMVCFunction<TReturn, TParam1, TParam2, TParam3, TParam4>, new();
        
        void Run<TCommand>() where TCommand : IMVCVoidFunction, new();
        void Run<TCommand, TParam1>(TParam1 param1) where TCommand : IMVCVoidFunction<TParam1>, new();
        void Run<TCommand, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TCommand : IMVCVoidFunction<TParam1, TParam2>, new();
        void Run<TCommand, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3) where TCommand : IMVCVoidFunction<TParam1, TParam2, TParam3>, new();
        void Run<TCommand, TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4) where TCommand : IMVCVoidFunction<TParam1, TParam2, TParam3, TParam4>, new();
    }
}
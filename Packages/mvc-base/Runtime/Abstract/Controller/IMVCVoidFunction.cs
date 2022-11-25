namespace MVC.Base.Runtime.Abstract.Controller
{
    public interface IMVCVoidFunction : IMVCCommandBody
    {
        void Execute();
    }
    
    public interface IMVCVoidFunction<TParam1> : IMVCCommandBody
    {
        void Execute(TParam1 param1);
    }
    
    public interface IMVCVoidFunction<TParam1, TParam2> : IMVCCommandBody
    {
        void Execute(TParam1 param1, TParam2 param2);
    }
    
    public interface IMVCVoidFunction<TParam1, TParam2, TParam3> : IMVCCommandBody
    {
        void Execute(TParam1 param1, TParam2 param2, TParam3 param3);
    }
    
    public interface IMVCVoidFunction<TParam1, TParam2, TParam3, TParam4> : IMVCCommandBody
    {
        void Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}